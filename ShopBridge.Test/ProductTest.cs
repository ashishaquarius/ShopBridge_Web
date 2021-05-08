using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopBridge.Domain.Models.DatabaseEntities;
using ShopBridge.Domain.Repositories.ProductRepo;
using ShopBridge_Web.Controllers.ApiControllers;

namespace ShopBridge.Test
{
    [TestClass]
    public class ProductTest
    {
        private readonly List<Product> _products = new List<Product>()
        {
            new Product{Id = 1, Name = "Iphone 11" },
            new Product{Id = 2, Name = "Iphone 10" },
        };
        private readonly Product _product = new Product()
        {
            Name = "Iphone 10",
            Description = "This is Iphone 10"
        };

        public ProductController ProductController;
        public Mock<IProductRepository> MoqRepositoryClass;
        [TestInitialize]
        public void before_each_test()
        {
            MoqRepositoryClass = new Mock<IProductRepository>();
            ProductController = new ProductController(MoqRepositoryClass.Object);
        }
        [TestMethod]
        public void GetProduct_WithValidData()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.GetProduct(2)).Returns(new Product { Id = 2, Name = "Test Name" });

            //Act
            IHttpActionResult result = ProductController.Get(2);

            //Assert
            if (result is OkNegotiatedContentResult<Product> resultProduct)
            {
                Assert.AreEqual(2, resultProduct.Content.Id);
                Assert.AreEqual("Test Name", resultProduct.Content.Name);
            }
        }

        [TestMethod]
        public void GetProduct_WithInValidData()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.GetProduct(2)).Returns(default(Product));

            //Act
            IHttpActionResult result = ProductController.Get(2);
            var resultProduct = result as OkNegotiatedContentResult<Product>;

            //Assert
            Assert.IsNull(resultProduct);
        }

        [TestMethod]
        public void GetAllProducts_WithData()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.GetAllProducts()).Returns(_products);

            //Act
            var result = ProductController.Get();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Product>));
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void GetAllProducts_WithOutData()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.GetAllProducts()).Returns(default(List<Product>));

            //Act
            var result = ProductController.Get();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddProduct_DataAddedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.AddProduct(_product)).Returns(true);

            //Act
            IHttpActionResult result = ProductController.AddProduct(_product);
            var resultProduct = result as OkNegotiatedContentResult<Product>;

            //Assert
            Assert.IsNotNull(resultProduct);
            Assert.AreEqual("Iphone 10", resultProduct.Content.Name);
            Assert.AreEqual("This is Iphone 10", resultProduct.Content.Description);
        }

        [TestMethod]
        public void AddProduct_DataNotAddedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.AddProduct(_product)).Returns(false);

            //Act
            var result = ProductController.AddProduct(_product);


            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Product is not added because of some error.", ((BadRequestErrorMessageResult)result).Message);
        }

        [TestMethod]
        public void UpdateProduct_RecordUpdatedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.UpdateProduct(2, _product)).Returns(_products);

            //Act
            IHttpActionResult result = ProductController.UpdateProduct(2, _product);
            var resultProduct = result as OkNegotiatedContentResult<List<Product>>;

            //Assert
            Assert.IsNotNull(resultProduct);
            Assert.IsTrue(resultProduct.Content.Any(x => x.Id == 2));
            Assert.AreEqual("Iphone 10", resultProduct.Content.FirstOrDefault(x => x.Id == 2)?.Name);
        }

        [TestMethod]
        public void UpdateProduct_RecordNotUpdatedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.UpdateProduct(3, _product)).Returns(default(List<Product>));

            //Act
            IHttpActionResult result = ProductController.UpdateProduct(2, _product);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void RemoveProduct_RecordRemovedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.RemoveProduct(3)).Returns(true);
            MoqRepositoryClass.Setup(x => x.GetAllProducts()).Returns(_products);

            //Act
            IHttpActionResult result = ProductController.DeleteProduct(3);

            //Assert
            if (result is OkNegotiatedContentResult<List<Product>> resultProduct && resultProduct.Content.Any())
            {
                Assert.IsFalse(resultProduct.Content.Any(x => x.Id == 3));
            }
        }

        [TestMethod]
        public void RemoveProduct_RecordNotRemovedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.RemoveProduct(3)).Returns(false);
            MoqRepositoryClass.Setup(x => x.GetAllProducts()).Returns(_products);

            //Act
            IHttpActionResult result = ProductController.DeleteProduct(3);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
