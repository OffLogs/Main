using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace Offlogs.Business.Api.Controller.Board.Log.Actions
{
    public class SetIsFavoriteRequest: IRequest
    {
        [Required]
        [IsPositive]
        public long LogId { get; set; }

        public bool IsFavorite { get; set; }
    }
}
