using SimpleCRM.DataAccess;
using SimpleCRM.Entities;
using SimpleCRM.UI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SimpleCRM.UI.Controllers
{
    public class CustomersController : Controller
    {
        //private db db = new db();
        private IUnitOfWork<db> uow;
        private IRepository<SalesOrderHeader> salesOrderHeaderRepository;
        private IRepository<SalesOrderDetail> salesOrderDetailRepository;
        private IRepository<Customer> customerRepository;
        private IRepository<Product> productRepository;
        public CustomersController()
        {
            uow = new UnitOfWork<db>();
            salesOrderHeaderRepository = new Repository<SalesOrderHeader>(uow.DbContext);
            customerRepository = new Repository<Customer>(uow.DbContext);
            productRepository = new Repository<Product>(uow.DbContext);
            salesOrderDetailRepository = new Repository<SalesOrderDetail>(uow.DbContext);
        }

        // GET: Sales
        public ActionResult Index()
        {
            var customers = customerRepository.GetAll().OrderBy(c=> c.LastName).ThenBy(c=>c.FirstName)
                                .Select(c=> new CustomerViewModel{ Id = c.CustomerID,
                                                                 CompanyName = c.CompanyName,
                                                                 FullName = c.LastName + ", " + c.FirstName,
                                                                 OrdersCount = c.SalesOrderHeaders.Count(o => o.CustomerID == c.CustomerID) } ).ToList();
                            
            return View(customers);
        }
        [Authorize]
        public ActionResult NewOrder()
        {
            var customerId = Convert.ToInt32(RouteData.Values["id"].ToString());
            var salesOrderViewModel = new SalesOrderViewModel();

            salesOrderViewModel.Order = new SalesOrderHeader();


            salesOrderViewModel.Order.CustomerID = customerId;
            salesOrderViewModel.Order.SalesOrderNumber = customerId.ToString() + DateTime.Now.ToLongTimeString();
            salesOrderViewModel.Order.ShipMethod = "Air";
            salesOrderViewModel.Order.ShipToAddress = new Address() { rowguid = Guid.NewGuid() };
            salesOrderViewModel.Order.BillToAddress = new Address() { rowguid = Guid.NewGuid() };
            salesOrderViewModel.Order.DueDate = DateTime.Now.AddDays(10);
            //Adding at least one item
            salesOrderViewModel.LineItems = new List<SalesOrderDetail>();

            for (int i = 0; i < 2; i++)
            {
                var sod = new SalesOrderDetail() { rowguid = Guid.NewGuid() };
                salesOrderViewModel.LineItems.Add(sod);
            }

            return View(salesOrderViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> NewOrder(SalesOrderViewModel salesOrderViewModel)
        {
            if (ModelState.IsValid)
            {
                var logFile = new StreamWriter(@"C:\Temp\log.txt");
                uow.DbContext.Database.Log = logFile.Write;

                salesOrderViewModel.Order.ModifiedDate = DateTime.Now;
                salesOrderViewModel.Order.OrderDate = DateTime.Now;
                salesOrderViewModel.Order.ShipDate = DateTime.Now.AddDays(15);
                salesOrderViewModel.Order.rowguid = Guid.NewGuid();
                salesOrderViewModel.Order.ShipToAddress.ModifiedDate = DateTime.Now;
                salesOrderViewModel.Order.BillToAddress.ModifiedDate = DateTime.Now;
                salesOrderHeaderRepository.Add(salesOrderViewModel.Order);
                
                foreach (var item in salesOrderViewModel.LineItems)
                {
                    if (item.ProductID > 0)
                    {
                        var product = await productRepository.GetByIdAsync(item.ProductID); 
                        item.ModifiedDate = DateTime.Now;
                        item.Product = product;
                        item.rowguid = Guid.NewGuid();
                        salesOrderDetailRepository.Add(item);
                    }
                }
                try
                {
                    await uow.SaveAsync();
                    logFile.Close();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                    logFile.Close();
                    return View(salesOrderViewModel);
                }




            }
            return View(salesOrderViewModel);
        }


    }
}