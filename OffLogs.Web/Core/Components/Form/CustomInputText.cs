using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace OffLogs.Web.Core.Components.Form
{
    public class CustomInputText<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> :  InputBase<TValue>
    {
        [Parameter] 
        public string? Label { get; set; }
        
        [Parameter] 
        public string? Description { get; set; }
        
        [Parameter] 
        public string? ParentClass { get; set; }
        
        [Parameter] 
        public string? ErrorMessage { get; set; }
        
        [Parameter] 
        public string? Placeholder { get; set; }
        
        [Parameter] 
        public int? MaxLength { get; set; }

        [Parameter]
        public bool IsMulti { get; set; } = false;
        
        [Parameter]
        public bool IsDisabled { get; set; } = false;

        /// <summary>
        /// Gets or sets the error message used when displaying an a parsing error.
        /// </summary>
        [Parameter]
        public string ParsingErrorMessage { get; set; } = "The {0} field must be a number.";
        
        private static readonly string _stepAttributeValue = GetStepAttributeValue();
        private static readonly bool _isNumber = IsNumber();

        private static string GetStepAttributeValue()
        {
            // Unwrap Nullable<T>, because InputBase already deals with the Nullable aspect
            // of it for us. We will only get asked to parse the T for nonempty inputs.
            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (targetType == typeof(int) ||
                targetType == typeof(long) ||
                targetType == typeof(short) ||
                targetType == typeof(float) ||
                targetType == typeof(double) ||
                targetType == typeof(decimal))
            {
                return "any";
            }
            else
            {
                throw new InvalidOperationException($"The type '{targetType}' is not a supported numeric type.");
            }
        }
        
        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var sequence = 0;
            
            builder.OpenElement(sequence++, "div");
            if (!string.IsNullOrEmpty(ParentClass))
            {
                builder.AddAttribute(sequence++, "class", ParentClass);
            }
            {
                if (!string.IsNullOrEmpty(Label))
                {
                    // Label
                    builder.OpenElement(sequence++, "label");
                    builder.AddAttribute(sequence++, "class", "input-group-prepend");
                    builder.AddContent(sequence++, Label);
                    builder.CloseElement();
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    builder.OpenElement(sequence++, "div");
                    builder.AddAttribute(sequence++, "class", "form-text");
                    builder.AddContent(sequence++, Description);
                    builder.CloseElement();
                }

                var bootstrapCssFiles = CssClass.Replace("invalid", "is-invalid");
                
                builder.OpenElement(sequence++, IsMulti ? "textarea" : "input");
                builder.AddMultipleAttributes(sequence++, AdditionalAttributes);
                builder.AddAttribute(sequence++, "class", $"{bootstrapCssFiles} form-control");
                builder.AddAttribute(sequence++, "value", BindConverter.FormatValue(CurrentValue));
                builder.AddAttribute(sequence++, "onchange", EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
                if (MaxLength.HasValue)
                {
                    builder.AddAttribute(sequence++, "maxlength", MaxLength.Value);
                }
                if (!string.IsNullOrEmpty(Placeholder))
                {
                    builder.AddAttribute(sequence++, "placeholder", Placeholder);
                }
                if (IsDisabled)
                {
                    builder.AddAttribute(sequence++, "disabled", "disabled");
                }

                if (_isNumber)
                {
                    builder.AddAttribute(sequence++, "step", _stepAttributeValue);    
                }
                builder.AddAttribute(sequence++,  "type", GetInputType());

                builder.CloseElement();

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    builder.OpenElement(sequence++, "div");
                    builder.AddAttribute(sequence++, "class", "invalid-feedback");
                    builder.AddContent(sequence++, ErrorMessage);
                    builder.CloseElement();
                }
            }
            builder.CloseElement();
        }

        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
            else
            {
                validationErrorMessage = string.Format(CultureInfo.InvariantCulture, ParsingErrorMessage, DisplayName ?? FieldIdentifier.FieldName);
                return false;
            }
        }
        
        /// <summary>
        /// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A string representation of the value.</returns>
        protected override string? FormatValueAsString(TValue? value)
        {
            // Avoiding a cast to IFormattable to avoid boxing.
            switch (value)
            {
                case null:
                    return null;

                case int @int:
                    return BindConverter.FormatValue(@int, CultureInfo.InvariantCulture);

                case long @long:
                    return BindConverter.FormatValue(@long, CultureInfo.InvariantCulture);

                case short @short:
                    return BindConverter.FormatValue(@short, CultureInfo.InvariantCulture);

                case float @float:
                    return BindConverter.FormatValue(@float, CultureInfo.InvariantCulture);

                case double @double:
                    return BindConverter.FormatValue(@double, CultureInfo.InvariantCulture);

                case decimal @decimal:
                    return BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture);

                default:
                    return value?.ToString();
            }
        }
        
        private static string GetInputType()
        {
            if (_isNumber)
            {
                return "number";
            }
            return "string";
        }
        
        private static bool IsNumber()
        {
            // Unwrap Nullable<T>, because InputBase already deals with the Nullable aspect
            // of it for us. We will only get asked to parse the T for nonempty inputs.
            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (targetType == typeof(int) ||
                targetType == typeof(long) ||
                targetType == typeof(short) ||
                targetType == typeof(float) ||
                targetType == typeof(double) ||
                targetType == typeof(decimal))
            {
                return true;
            }
            return false;
        }
    }
}
