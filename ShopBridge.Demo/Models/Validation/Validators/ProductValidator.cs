using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop_Bridge.Domain.Models.DatabaseEntities;
using Shop_Bridge.Domain.Models.Validation.Common;

namespace Shop_Bridge.Domain.Models.Validation.Validators
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
