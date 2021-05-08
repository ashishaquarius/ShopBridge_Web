using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShopBridge.Domain.Common;
using ShopBridge.Domain.Models.DatabaseEntities;
using ShopBridge.Domain.Repositories.ProductRepo;
using ShopBridge_Web.Controllers.ApiControllers;

namespace ShopBridge.Test
{
    [TestClass]
    public class ProductTest
    {
        #region Private variables and Initialization
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
            ProductController = new ProductController(MoqRepositoryClass.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        #endregion

        #region TestMethods

        [TestMethod]
        public async Task GetAllProducts_WithData()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.GetAllProducts()).Returns(_products);

            //Act
            HttpResponseMessage result = await ProductController.Get();

            //Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out List<Product> products));
            Assert.IsTrue(products.Count > 0);
        }

        [TestMethod]
        public async Task GetAllProducts_WithOutData()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.GetAllProducts()).Returns(default(List<Product>));

            //Act
            HttpResponseMessage result = await ProductController.Get();

            //Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.IsTrue(result.TryGetContentValue(out IEnumerable<BrokenRule> brokenRules));
            Assert.IsTrue(brokenRules.Any(r => r.Description.Contains("Product list is empty.")));
        }

        [TestMethod]
        public async Task GetProduct_WithValidData()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.GetProduct(2)).Returns(new Product { Id = 2, Name = "Test Name" });

            //Act
            HttpResponseMessage result = await ProductController.Get(2);

            //Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out Product product));
            Assert.AreEqual(2, product.Id);
            Assert.AreEqual("Test Name", product.Name);
        }

        [TestMethod]
        public async Task GetProduct_WithInValidData()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.GetProduct(2)).Returns(default(Product));

            //Act
            HttpResponseMessage result = await ProductController.Get(2);

            //Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.IsTrue(result.TryGetContentValue(out IEnumerable<BrokenRule> brokenRules));
            Assert.IsTrue(brokenRules.Any(r => r.Description.Contains("No product found for the given id.")));
        }

        [TestMethod]
        public async Task AddProduct_DataAddedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.AddProduct(_product)).Returns(true);

            //Act
            HttpResponseMessage result = await ProductController.AddProduct(_product);

            //Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out Product product));
            Assert.AreEqual("Iphone 10", product.Name);
            Assert.AreEqual("This is Iphone 10", product.Description);
        }

        [TestMethod]
        public async Task AddProduct_DataNotAddedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.AddProduct(_product)).Returns(false);

            //Act
            HttpResponseMessage result = await ProductController.AddProduct(_product);

            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.IsTrue(result.TryGetContentValue(out IEnumerable<BrokenRule> brokenRules));
            Assert.IsTrue(brokenRules.Any(r => r.Description.Contains("Product can't be added successfully.")));
        }

        [TestMethod]
        public async Task AddProduct_ApiProductModelIsNull()
        {
            //Arrange

            //Act
            HttpResponseMessage result = await ProductController.AddProduct(null);

            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.IsTrue(result.TryGetContentValue(out IEnumerable<BrokenRule> brokenRules));
            Assert.IsTrue(brokenRules.Any(r => r.Description.Contains("No product to add.")));
        }

        [TestMethod]
        public async Task UpdateProduct_RecordUpdatedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.UpdateProduct(2, _product)).Returns(_products);

            //Act
            HttpResponseMessage result = await ProductController.UpdateProduct(2, _product);

            //Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out List<Product> products));
            Assert.IsTrue(products.Any(x => x.Id == 2));
            Assert.AreEqual("Iphone 10", products.FirstOrDefault(x => x.Id == 2)?.Name);
        }

        [TestMethod]
        public async Task UpdateProduct_RecordNotUpdatedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.UpdateProduct(3, _product)).Returns(default(List<Product>));

            //Act
            HttpResponseMessage result = await ProductController.UpdateProduct(3, _product);

            //Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.IsTrue(result.TryGetContentValue(out IEnumerable<BrokenRule> brokenRules));
            Assert.IsTrue(brokenRules.Any(r => r.Description.Contains("No products were found.")));
        }

        [TestMethod]
        public async Task RemoveProduct_RecordRemovedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.RemoveProduct(3)).Returns(true);
            MoqRepositoryClass.Setup(x => x.GetAllProducts()).Returns(_products);

            //Act
            HttpResponseMessage result = await ProductController.DeleteProduct(3);

            //Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out List<Product> products));
            Assert.IsFalse(products.Any(x => x.Id == 3));
        }

        [TestMethod]
        public async Task RemoveProduct_RecordNotRemovedSuccessfully()
        {
            //Arrange
            MoqRepositoryClass.Setup(x => x.RemoveProduct(3)).Returns(false);
            MoqRepositoryClass.Setup(x => x.GetAllProducts()).Returns(_products);

            //Act
            HttpResponseMessage result = await ProductController.DeleteProduct(3);

            //Assert
            Assert.IsTrue(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.IsTrue(result.TryGetContentValue(out IEnumerable<BrokenRule> brokenRules));
            Assert.IsTrue(brokenRules.Any(r => r.Description.Contains("No product was found to delete for the given id.")));
        }

        #endregion


    }
}
