using System.Collections.Generic;
using System.Linq;
using ShopBridge.Domain.Common;
using ShopBridge.Domain.Models.DatabaseEntities;

namespace ShopBridge.Domain.Models.Validation.Validators
{
    public class ProductValidator : IValidator<Product>
    {
        public bool IsValid(Product entity)
        {
            return !BrokenRules(entity).Any();
        }

        public IEnumerable<BrokenRule> BrokenRules(Product entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
                yield return new BrokenRule("Product Name", "Must include an Product Name.");

            if (string.IsNullOrEmpty(entity.Description))
                yield return new BrokenRule("Product Description", "Must include an Product Description.");

            if (entity.Price < 1)
                yield return new BrokenRule("Product Price", "Product must have a valid price.");

        }
    }
}
