using System.Collections.Generic;

namespace ShopBridge.Domain.Common
{
    public interface IValidator<in T>
    {
        bool IsValid(T entity);
        IEnumerable<BrokenRule> BrokenRules(T entity);
    }
}
