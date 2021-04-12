using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Business.Common.Models.Api.Request.Board
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