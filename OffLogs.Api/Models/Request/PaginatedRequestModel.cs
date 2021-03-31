using OffLogs.Business.Mvc.Attribute.Validation;
using ServiceStack.DataAnnotations;

namespace OffLogs.Api.Models.Request
{
    public record PaginatedRequestModel
    {
        [Required]
        [IsPositive]
        public int Page { get; set; }
    }
}