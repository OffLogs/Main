using OffLogs.Business.Mvc.Attribute.Validation;
using ServiceStack.DataAnnotations;

namespace OffLogs.Api.Models.Request.Board
{
    public record ApplicationAddModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public ApplicationAddModel()
        {
        }
    }
}