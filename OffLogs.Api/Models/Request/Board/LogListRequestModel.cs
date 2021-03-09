using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Mvc.Attribute.Validation;

namespace OffLogs.Api.Models.Request.Board
{
    public record LogListRequestModel: PaginatedRequestModel
    {
        [Required]
        [IsPositive]
        public long ApplicationId { get; set; }

        public LogListRequestModel()
        {
        }
    }
}