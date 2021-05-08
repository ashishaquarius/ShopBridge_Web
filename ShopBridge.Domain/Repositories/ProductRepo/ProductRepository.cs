using System.Collections.Generic;
using System.Linq;
using ShopBridge.Domain.Common;
using ShopBridge.Domain.Models.DatabaseEntities;
using ShopBridge.Domain.Models.Mappings;

namespace ShopBridge.Domain.Repositories.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IValidator<Product> _productValidator;
        private readonly ShopBridgeDbContext _dbContext = new ShopBridgeDbContext();

        public ProductRepository(IValidator<Product> productValidator)
        {
            _productValidator = productValidator;
        }

        public bool AddProduct(Product product)
        {
            if (!_productValidator.IsValid(product))
            {
                    throw new PersistenceValidationException("Validation Error",
                        _productValidator.BrokenRules(product));
            }
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return true;
        }

        public List<Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }

        public Product GetProduct(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            return product;
        }

        public bool RemoveProduct(int id)
        {
            var product = GetProduct(id);
            if (product == null)
            {
                return false;
            }
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
            return true;
        }

        public List<Product> UpdateProduct(int id, Product product)
        {
            if (RemoveProduct(id) == true)
            {
                AddProduct(product);
                return GetAllProducts();
            }
            return GetAllProducts();

        }
    }


}
