using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleCRM.DataAccess;
using SimpleCRM.Entities;
using SimpleCRM.UI.Controllers;
using SimpleCRM.UnitTests.Mocks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleCRM.UI.Tests.Controllers
{
    [TestClass]
    public class TestProductController
    {
        [TestMethod]
        public void Test_Products_Index_ShouldReturnAViewAndViewHaveProductsData()
        {
            //AAA
            //Arrange:
            var uowMock = setUpUnitOfWorkMock();
            var controller = new ProductsController(uowMock.Object);
            controller.ControllerContext = new ControllerContext()
            {
                Controller = controller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            //Act:
            var taskViewResult = controller.Index();
            var viewResult = taskViewResult.Result as ViewResult;
            //Assert:
            Assert.IsNotNull(viewResult.Model);
        }

        private static Mock<IUnitOfWork<db>> setUpUnitOfWorkMock()
        {
            var products = new List<Product>();
            for (int i = 0; i < 10; i++)
            {
                var product = new Product { ProductID = i, Name = i.ToString() };
                products.Add(product);
            }

            var data = products.AsQueryable();

            var mockDbSet = new Mock<DbSet<Product>>();

            mockDbSet.As<IDbAsyncEnumerable<Product>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<Product>(data.GetEnumerator()));
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<Product>(data.Provider));
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockDbContext = new Mock<db>();
            mockDbContext.Setup(c => c.Products).Returns(mockDbSet.Object);
            mockDbContext.Setup(c => c.Set<Product>()).Returns(mockDbSet.Object);
            var uowMock = new Mock<IUnitOfWork<db>>();
            uowMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            uowMock.Setup(uow => uow.DbContext).Returns(mockDbContext.Object);
            return uowMock;
        }
    }
}
