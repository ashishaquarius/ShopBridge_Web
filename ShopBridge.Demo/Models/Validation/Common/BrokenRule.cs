using Newtonsoft.Json;

namespace Shop_Bridge.Domain.Models.Validation.Common
{
    public class BrokenRule
    {
        [JsonConstructor]
        public BrokenRule(string name, string description)
        {
            Name = name;
            Description = description;
            ErrorCategory = BrokenRuleEnum.Validation.ToString();
        }
        public BrokenRule(string errorCategory, string name, string description)
        {
            ErrorCategory = errorCategory;
            Name = name;
            Description = description;
        }

        public string ErrorCategory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
