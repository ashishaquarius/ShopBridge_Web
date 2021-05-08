using System;
using Newtonsoft.Json;

namespace Shop_Bridge.Domain.Models.DatabaseEntities
{
    public class Product
    {
        [JsonIgnore] // Don't want to expose this property to api payload
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
