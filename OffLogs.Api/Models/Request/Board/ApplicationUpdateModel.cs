using OffLogs.Business.Mvc.Attribute.Validation;
using ServiceStack.DataAnnotations;

namespace OffLogs.Api.Models.Request.Board
{
    public record ApplicationUpdateModel: ApplicationAddModel
    {
        [Required]
        public long Id { get; set; }

        public ApplicationUpdateModel()
        {
        }
    }
}