﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule
{
    public class SetRuleRequest : IRequest<NotificationRuleDto>
    {
        [IsPositive(AllowZero = true)]
        public long? Id { get; set; }

        [Required]
        [IsPositive(AllowZero = false)]
        public long MessageId { get; set; }
        
        [Required]
        [IsPositive(AllowZero = false)]
        public long? ApplicationId { get; set; }
        
        [Required]
        [EnumDataType(typeof(NotificationType))]
        public string Type { get; set; }
        
        [Required]
        [EnumDataType(typeof(LogicOperatorType))]
        public string LogicOperator { get; set; }
        
        [Required]
        [IsPositive(AllowZero = false)]
        [Range(300, 2_678_400)]
        public int Period { get; set; }

        public ICollection<SetConditionRequest> Conditions { get; set; } = new List<SetConditionRequest>();
    }
}