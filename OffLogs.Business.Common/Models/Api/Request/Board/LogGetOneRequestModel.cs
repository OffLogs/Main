using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Business.Common.Models.Api.Request.Board
{
    public record LogGetOneRequestModel
    {
        [Required]
        [IsPositive(AllowZero = true)]
        public long Id { get; set; }

        public LogGetOneRequestModel()
        {
        }
    }
}