using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridge.Domain.Common
{
    public class PersistenceValidationException : Exception
    {
        public IEnumerable<BrokenRule> BrokenRules;

        public PersistenceValidationException(string message, IEnumerable<BrokenRule> brokenRules)
            : base(message)
        {
            BrokenRules = brokenRules;
        }
    }
}
