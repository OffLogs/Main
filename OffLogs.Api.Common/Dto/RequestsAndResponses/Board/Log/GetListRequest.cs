using System;
using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Log
{
    public class GetListRequest : IRequest<PaginatedListDto<LogListItemDto>>
    {
        [Required]
        [IsPositive]
        public int Page { get; set; }
        
        [Required]
        [StringLength(2048)]
        [IsBase64]
        public string PrivateKeyBase64 { get; set; }

        [Required]
        [IsPositive]
        public long ApplicationId { get; set; }

        public LogLevel? LogLevel { get; set; }

        public bool IsFavorite { get; set; } = false;

        public DateTime? CreateTimeFrom { get; set; }
        
        public DateTime? CreateTimeTo { get; set; }
    }
}
