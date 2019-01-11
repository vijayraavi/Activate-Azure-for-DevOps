using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SimpleCRM.Entities;
using System.Drawing;
using System.IO;
using SimpleCRM.UI.Models;
using SimpleCRM.DataAccess;

namespace SimpleCRM.UI.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Product
        private IUnitOfWork<db> uow;
        private Repository<Product> productRepository;
        private Repository<ProductCategory> productCategoryRepository;
        private Repository<ProductModel> productModelRepository;
        public ProductsController(IUnitOfWork<db> UnitOfWork)
        {
            uow = UnitOfWork;
            productRepository = new Repository<Product>(uow.DbContext);
            productCategoryRepository = new Repository<ProductCategory>(uow.DbContext);
            productModelRepository = new Repository<ProductModel>(uow.DbContext);
        }
        public async Task<ActionResult> Index()
        {
            int pageindex = (RouteData.Values["pageindex"] == null) ? 0 : Convert.ToInt32(RouteData.Values["pageindex"]);
            int pagesize = (RouteData.Values["pagesize"] == null) ? 10 : Convert.ToInt32(RouteData.Values["pagesize"]);
            //var products = await dbContext.Products.Include("ProductCategory").
            //OrderBy(p => p.Name).Skip(pageindex * pagesize).Take(pagesize).ToListAsync();
            var products = await productRepository.GetAll()
                                    .OrderBy(p => p.Name).Skip(pageindex * pagesize).Take(pagesize).ToListAsync();
            return View(products);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include =
                                        "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate")]
                                        Product product)
        {
            if (ModelState.IsValid)
            {
                //dbContext.Products.Add(product);
                //await dbContext.SaveChangesAsync();
                productRepository.Add(product);
                await uow.SaveAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await productRepository.GetByIdAsync(id.Value); //await dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await productRepository.GetByIdAsync(id.Value);//await dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Product product = await productRepository.GetByIdAsync(id);//await dbContext.Products.FindAsync(id);
                productRepository.Delete(product);//dbContext.Products.Remove(product);
                await uow.SaveAsync();//await dbContext.SaveChangesAsync();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var productCategories = (from c in dbContext.ProductCategories
            //                         orderby c.Name
            //                         select new { Id = c.ProductCategoryID, Name = c.Name }).ToList();
            ViewBag.ProductCategories = productCategoryRepository.GetAll().OrderBy(c=>c.Name).Select(c=>new { Id = c.ProductCategoryID, Name = c.Name }).ToList();

            //var productModels = (from c in dbContext.ProductModels
            //                     orderby c.Name
            //                     select new { Id = c.ProductModelID, Name = c.Name }).ToList();
            var productModels = productModelRepository.GetAll().OrderBy(c => c.Name).Select(c => new { Id = c.ProductModelID, Name = c.Name }).ToList();

            ViewBag.ProductModels = productModels;


            Product product = await productRepository.GetByIdAsync(id.Value);//dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductID,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryID,ProductModelID,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                //dbContext.Entry(product).State = EntityState.Modified;
                product.ModifiedDate = DateTime.Now;
                uow.DbContext.Entry(product).State = EntityState.Modified;
                await uow.SaveAsync();//dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }



        public JsonResult ProductLookup(string q)
        {
            //var products = (from p in dbContext.Products
            //                where p.Name.Contains(q)
            //                orderby p.Name ascending
            //                select new ProductSummary() { Id = p.ProductID, Name = p.Name, Price = p.ListPrice, ProductNumber = p.ProductNumber }).Take(20).ToList();
           
            var products = productRepository
                            .SearchFor(p => p.Name.Contains(q))
                            .OrderBy(p => p.Name)
                            .Select(p => new ProductSummary() { Id = p.ProductID, Name = p.Name, Price = p.ListPrice, ProductNumber = p.ProductNumber }).ToList();

            return Json(products, JsonRequestBehavior.AllowGet);
        }



        protected override void Dispose(bool disposing)
        {
            //dbContext.Dispose();
            uow.DbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}
