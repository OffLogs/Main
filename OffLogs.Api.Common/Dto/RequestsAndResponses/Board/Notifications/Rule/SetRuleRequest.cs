﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Api.Requests.Abstractions;
using OffLogs.Api.Common.Dto.Entities;
using OffLogs.Api.Common.Resources;
using OffLogs.Business.Common.Constants.Notificatiions;
using OffLogs.Business.Common.Mvc.Attribute.Validation;

namespace OffLogs.Api.Common.Dto.RequestsAndResponses.Board.Notifications.Rule
{
    public class SetRuleRequest : IRequest<NotificationRuleDto>
    {
        [IsPositive(AllowZero = true)]
        public long? Id { get; set; }

        [
            Required,
            IsPositive(AllowZero = false)
        ]
        public long MessageId { get; set; }
        
        [IsPositive(AllowZero = false)]
        public long? ApplicationId { get; set; }
        
        [
            Required, 
            EnumDataType(typeof(NotificationType))
        ]
        public string Type { get; set; }
        
        [
            Required,
            EnumDataType(typeof(LogicOperatorType)),
            Display(Name = "Notification_LogicOperator", ResourceType = typeof(RequestResources))
        ]
        public string LogicOperator { get; set; }

        [
            Required,
            IsPositive(AllowZero = false),
            Range(300, 2_678_400)
        ]
        public int Period { get; set; } = 300;

        [
            Required,
            MinLength(1),
            ValidateListModels
        ]
        public ICollection<SetConditionRequest> Conditions { get; set; } = new List<SetConditionRequest>();

        public void Fill(NotificationRuleDto item)
        {
            Id = item?.Id;
            if (item != null)
            {
                MessageId = item.MessageTemplate.Id;
                ApplicationId = item.Application?.Id;
                Type = item.Type.ToString();
                LogicOperator = item.LogicOperator.ToString();
                Period = item.Period;
                Conditions = item.Conditions.Select(condition => new SetConditionRequest(condition)).ToList();
            }
        }
    }
}
