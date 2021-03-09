using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Mvc.Attribute.Validation;

namespace OffLogs.Api.Models.Request
{
    public record PaginatedRequestModel
    {
        [Required]
        [IsPositive]
        public int Page { get; set; }
    }
}