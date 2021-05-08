using System.Collections.Generic;

namespace Shop_Bridge.Domain.Models.Validation.Common
{
    public interface IValidator<in T>
    {
        bool IsValid(T entity);
        IEnumerable<BrokenRule> BrokenRules(T entity);
    }
}
