using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace OffLogs.Web.Core.Components.Form
{
    public class CustomInputText : InputBase<string?>
    {
        [Parameter] public string? Label { get; set; }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var sequence = 0;
            
            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "input-group mb-3");
            {
                builder.OpenElement(sequence++, "div");
                builder.AddAttribute(sequence++, "class", "input-group-prepend");
                {
                    // Label
                    builder.OpenElement(sequence++, "span");
                    builder.AddAttribute(sequence++, "class", "input-group-text");
                    builder.AddContent(sequence++, Label);
                    builder.CloseElement();
                }
                builder.CloseElement();
            
                builder.OpenElement(sequence++, "input");
                builder.AddMultipleAttributes(sequence++, AdditionalAttributes);
                builder.AddAttribute(sequence++, "class", CssClass);
                builder.AddAttribute(sequence++, "value", BindConverter.FormatValue(CurrentValue));
                builder.AddAttribute(sequence++, "onchange", EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
                builder.CloseElement();
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