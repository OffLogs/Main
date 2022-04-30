using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace OffLogs.Web.Core.Components.Form
{
    public class CustomInputText : InputBase<string?>
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

        /// <inheritdoc />
        protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }
    }
}
