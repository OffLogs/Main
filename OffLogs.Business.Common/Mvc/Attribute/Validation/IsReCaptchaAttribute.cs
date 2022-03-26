using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using OffLogs.Business.Common.Resources;
using OffLogs.Business.Common.Services.Web.ReCaptcha;

namespace OffLogs.Business.Common.Mvc.Attribute.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class IsReCaptchaAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return GetError(validationContext);
        }

        if (value is string stringValue)
        {
            var reCaptchaService = validationContext.GetRequiredService<IReCaptchaService>();
            if (reCaptchaService == null) throw new ArgumentNullException(nameof(IReCaptchaService));
            
            var isValid = reCaptchaService.ValidateAsync(stringValue).Result;
            return isValid ? ValidationResult.Success : GetError(validationContext);
        }
        return GetError(validationContext);
    }

    private ValidationResult GetError(ValidationContext validationContext) => new(
        string.Format(RG.Error_IncorrectReCaptchaToken, validationContext.DisplayName)
    );
}
