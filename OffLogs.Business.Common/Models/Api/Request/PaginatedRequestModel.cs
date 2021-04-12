using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Business.Common.Models.Api.Request
{
    public record PaginatedRequestModel
    {
        [Required]
        [IsPositive]
        public int Page { get; set; }
    }
}