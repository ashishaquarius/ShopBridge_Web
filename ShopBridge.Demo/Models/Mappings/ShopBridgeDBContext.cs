using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop_Bridge.Domain.Models.DatabaseEntities;

namespace Shop_Bridge.Domain.Models.Mappings
{
    public class ShopBridgeDbContext : DbContext
    {
        public ShopBridgeDbContext() : base("name=ShopBridgeDBContext")
        {
            Database.SetInitializer<ShopBridgeDbContext>(new DropCreateDatabaseIfModelChanges<ShopBridgeDbContext>());
        }

        public DbSet<Product> Products { get; set; }
    }
}
