using System.Data.Entity;
using ShopBridge.Domain.Models.DatabaseEntities;

namespace ShopBridge.Domain.Models.Mappings
{
    public class ShopBridgeDbContext : DbContext
    {
        public ShopBridgeDbContext() : base("name=ShopBridgeDbContext")
        {
            Database.SetInitializer<ShopBridgeDbContext>(new DropCreateDatabaseIfModelChanges<ShopBridgeDbContext>());
        }

        public DbSet<Product> Products { get; set; }
    }
}
