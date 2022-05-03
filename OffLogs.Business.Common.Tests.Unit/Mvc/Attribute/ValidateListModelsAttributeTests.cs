using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using OffLogs.Business.Common.Mvc.Attribute.Validation;
using Xunit;

namespace OffLogs.Business.Common.Tests.Unit.Mvc.Attribute
{
    class ListItemToValidate
    {
        [StringLength(2)]
        public string SomeString { get; set; }
    }
    
    class ModelToValidate
    {
        [ValidateListModels]
        public ICollection<ListItemToValidate> ListWithModels { get; set; } = new List<ListItemToValidate>();
        
        [ValidateListModels]
        public ListItemToValidate[] ArrayWithModels { get; set; }
    }

    public class GetDisplayName
    {

        [Fact]
        public void ShouldReceiveErrorWhileCollectionValidation()
        {
            var model = new ModelToValidate();
            model.ListWithModels.Add(
                new ListItemToValidate() { SomeString = "33333" }    
            );
            
            Assert.NotNull(Validate(model));
        }
        
        [Fact]
        public void ShouldReceiveErrorWhileArrayValidation()
        {
            var model = new ModelToValidate();
            model.ArrayWithModels = new[]
            {
                new ListItemToValidate() { SomeString = "33333" }
            };
            
            Assert.NotNull(Validate(model));
        }

        private string? Validate(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(model, context, results, true);
            if (!isValid)
            {;
                return results.FirstOrDefault()?.ErrorMessage;
            }

            return null;
        }
    }
}
