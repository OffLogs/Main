using System.ComponentModel.DataAnnotations;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Business.Common.Models.Api.Request.Board
{
    public record LogSetFavoriteRequestModel
    {
        [Required]
        [IsPositive]
        public long LogId { get; set; }

        public bool IsFavorite { get; set; }
        
        public LogSetFavoriteRequestModel()
        {
        }
    }
}