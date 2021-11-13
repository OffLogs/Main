using System;
using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Common.Resources;

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

                byte[] buffer = new byte[((base64.Length * 3) + 3) / 4 -
                                         (base64.Length > 0 && base64[base64.Length - 1] == '=' ?
                                             base64.Length > 1 && base64[base64.Length - 2] == '=' ?
                                                 2 : 1 : 0)];
                var isNotBase64 = Convert.TryFromBase64String(base64, buffer, out _);
                if (!isNotBase64)
                {
                    return new ValidationResult(String.Format(RG.Error_IncorrectBase64, validationContext.DisplayName));    
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(String.Format(RG.Error_FieldContainsIncorrectIPv4Address, validationContext.DisplayName));
        }
    }
}
