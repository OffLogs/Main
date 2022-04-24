﻿using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants.Permissions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Settings.NotificationMessage
{
    public class SetMessageRequest : IRequest<NotificationMessageDto>
    {
        [IsPositive(AllowZero = true)]
        public long? Id { get; set; }
        
        [Required]
        [StringLength(512, MinimumLength = 1)]
        public string Subject { get; set; }
        
        [Required]
        [StringLength(5048, MinimumLength = 1)]
        public string Body { get; set; }
    }
}
