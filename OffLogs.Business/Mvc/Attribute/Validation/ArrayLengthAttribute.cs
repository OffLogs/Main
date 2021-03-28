using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Resources;

namespace OffLogs.Business.Mvc.Attribute.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArrayLengthAttribute : ValidationAttribute
    {
        public int Max { get; }

        public ArrayLengthAttribute(int Max)
        {
            Max = 0;
            this.Max = Max;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is IList arrayToValidate))
            {
                return ValidationResult.Success;
            }
            if (arrayToValidate.Count > Max)
            {
                return new ValidationResult(
                    string.Format(
                        RG.Error_ArrayIsTooBig, 
                        validationContext.DisplayName, 
                        Max
                    )
                );
            }
            return ValidationResult.Success;
        }
    }
}