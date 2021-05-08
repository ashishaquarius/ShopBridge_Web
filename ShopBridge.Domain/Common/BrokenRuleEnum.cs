using System.ComponentModel;

namespace ShopBridge.Domain.Common
{
    public enum BrokenRuleEnum
    {
        //use this for general exceptions.
        [Description("General")]
        General,

        // Use this for pre-processing checks; e.g. before any database activity
        [Description("General")]
        Validation,

        // Use this for any post checks cause from a data related issue after trying to process a request
        [Description("Data")]
        Data
    }
}
