using System;
using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Common.Resources;
using OffLogs.Business.Common.Utils;

namespace OffLogs.Business.Common.Mvc.Attribute.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsBase64Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            if (value is string)
            {
                var base64 = value.ToString();
                if (!Base64Utils.IsValidBase64(base64))
                {
                    return new ValidationResult(String.Format(RG.Error_IncorrectBase64, validationContext.DisplayName));    
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(String.Format(RG.Error_FieldContainsIncorrectIPv4Address, validationContext.DisplayName));
        }
    }
}
