using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Business.Common.Models.Api.Request.Board
{
    public record LogListRequestModel: PaginatedRequestModel
    {
        [Required]
        [IsPositive]
        public long ApplicationId { get; set; }
        
        public LogLevel? LogLevel { get; set; }

        public LogListRequestModel()
        {
        }
    }
}