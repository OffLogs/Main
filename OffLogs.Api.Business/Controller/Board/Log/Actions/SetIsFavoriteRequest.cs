using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Business.Controller.Board.Log.Actions
{
    public class SetIsFavoriteRequest: IRequest
    {
        [Required]
        [IsPositive]
        public long LogId { get; set; }

        public bool IsFavorite { get; set; }
    }
}
