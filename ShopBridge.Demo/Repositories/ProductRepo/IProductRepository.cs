using Shop_Bridge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop_Bridge.Domain.Models.DatabaseEntities;

namespace Shop_Bridge.Domain.Repositories.ProductRepo
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
