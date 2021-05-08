using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShopBridge.Domain.Models.DatabaseEntities;
using ShopBridge.Domain.Repositories.ProductRepo;
using Swashbuckle.Swagger.Annotations;

namespace ShopBridge_Web.Controllers.ApiControllers
{
    
    public class ProductController : ApiController
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("api/product/GetProducts")]
        public IEnumerable<Product> Get()
        {
            return _productRepository.GetAllProducts();
        }

        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Product_AdditionalApi" })] // Just to show use of SwaggerOperation Tag
        [Route("api/product/GetProductById/{id}")]
        public IHttpActionResult Get(int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [Route("api/product/AddProduct")]
        public IHttpActionResult AddProduct(Product product)
        {
            var result = _productRepository.AddProduct(product);
            if (result == true)
            {
                return Ok(product);
            }
            return BadRequest("Product is not added because of some error.");
        }

        [HttpPut]
        [Route("api/product/UpdateProduct/{id}")]
        public IHttpActionResult UpdateProduct(int id, Product product)
        {
            var products = _productRepository.UpdateProduct(id, product);
            if (products != null)
            {
                return Ok(products);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("api/product/DeleteProduct/{id}")]
        public IHttpActionResult DeleteProduct(int id)
        {
            var result = _productRepository.RemoveProduct(id);
            if (result == true)
            {
                return Ok(_productRepository.GetAllProducts());
            }
            return NotFound();
        }
    }
}