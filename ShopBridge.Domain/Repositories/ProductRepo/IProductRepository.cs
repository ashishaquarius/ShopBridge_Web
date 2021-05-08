using System.Collections.Generic;
using ShopBridge.Domain.Models.DatabaseEntities;

namespace ShopBridge.Domain.Repositories.ProductRepo
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product GetProduct(int id);
        bool AddProduct(Product product);
        bool RemoveProduct(int id);
        List<Product> UpdateProduct(int id, Product product);
    }
}
