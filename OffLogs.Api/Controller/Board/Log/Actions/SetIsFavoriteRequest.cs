using Api.Requests.Abstractions;
using OffLogs.Api.Dto;
using OffLogs.Api.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Mvc.Attribute.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Api.Controller.Board.Log.Actions
{
    public class SetIsFavoriteRequest: IRequest
    {
        [Required]
        [IsPositive]
        public long LogId { get; set; }

        public bool IsFavorite { get; set; }
    }
}
