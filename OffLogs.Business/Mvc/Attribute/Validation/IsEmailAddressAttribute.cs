using System;
using System.ComponentModel.DataAnnotations;

namespace OffLogs.Business.Mvc.Attribute.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsEmailAddressAttribute : RegularExpressionAttribute
    {
        public IsEmailAddressAttribute() : base(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
        {  
        }
    }
}
