using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using ShopBridge.Domain.Common;
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

        /// <summary>
        /// Returns the list of all Products
        /// </summary>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/product/GetProducts")]
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                var productList = await
                    Task.Run(() => _productRepository.GetAllProducts());
                if (productList == null)
                {
                    throw new PersistenceValidationException("Validation Error",
                        new List<BrokenRule> { new BrokenRule("Not Data Found", "Product list is empty.") });
                }
                return Request.CreateResponse(HttpStatusCode.OK, productList);
            }
            catch (PersistenceValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.BrokenRules);
            }
        }

        /// <summary>
        /// Returns the product for the given id. Just to show use of SwaggerOperation Tag
        /// </summary>
        /// <remarks>
        /// Parameters: 
        ///     id (required)
        /// </remarks>
        [System.Web.Http.HttpGet]
        [SwaggerOperation(Tags = new[] { "Product_AdditionalApi" })]
        [System.Web.Http.Route("api/product/GetProductById/{id}")]
        public async Task<HttpResponseMessage> Get(int id)
        {
            try
            {
                var product = await
                    Task.Run(() => _productRepository.GetProduct(id));
                if (product == null)
                {
                    throw new PersistenceValidationException("Validation Error",
                        new List<BrokenRule> { new BrokenRule("Not Found", "No product found for the given id.") });
                }

                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
            catch (PersistenceValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.BrokenRules);
            }


        }

        /// <summary>
        /// Adds a product record for the given payload.
        /// </summary>
        /// <remarks>
        /// Parameters:
        /// 
        ///     Name (required)
        /// 
        ///     Description (required)
        /// 
        ///     Price (required and greater than zero)
        /// </remarks>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/product/AddProduct")]
        public async Task<HttpResponseMessage> AddProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    throw new PersistenceValidationException("Validation Error",
                        new List<BrokenRule> { new BrokenRule("Empty", "No product to add.") });
                }

                var isProductAdded = await
                    Task.Run(() => _productRepository.AddProduct(product));

                if (isProductAdded == false)
                {
                    throw new PersistenceValidationException("Database Error",
                        new List<BrokenRule> { new BrokenRule("Save Changes Error", "Product can't be added successfully.") });
                }

                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
            catch (PersistenceValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.BrokenRules);
            }

        }

        /// <summary>
        /// Update a product record for the given payload and id.
        /// </summary>
        /// <remarks>
        /// Parameters:
        /// 
        ///     id (required)
        /// </remarks>
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/product/UpdateProduct/{id}")]
        public async Task<HttpResponseMessage> UpdateProduct(int id, Product product)
        {
            try
            {
                var products = await
                    Task.Run(() => _productRepository.UpdateProduct(id, product));
                if (products == null)
                {
                    throw new PersistenceValidationException("Validation Error",
                        new List<BrokenRule> { new BrokenRule("Empty", "No products were found.") });
                }
                return Request.CreateResponse(HttpStatusCode.OK, products);
            }
            catch (PersistenceValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.BrokenRules);
            }
        }

        /// <summary>
        /// Deletes a product record for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="PersistenceValidationException"></exception>
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/product/DeleteProduct/{id}")]
        public async Task<HttpResponseMessage> DeleteProduct(int id)
        {
            try
            {
                var result = await 
                    Task.Run( () => _productRepository.RemoveProduct(id));
                if (result == false)
                {
                    throw new PersistenceValidationException("Validation Error",
                        new List<BrokenRule> { new BrokenRule("Empty", "No product was found to delete for the given id.") });
                }
                return Request.CreateResponse(HttpStatusCode.OK, _productRepository.GetAllProducts());
            }
            catch (PersistenceValidationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.BrokenRules);
            }
        }
    }
}