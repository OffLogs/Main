using System;
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
        private const int defaultPeriod = 300;
        private const NotificationType defaultType = NotificationType.Email;
        
        [IsPositive(AllowZero = true)]
        public long? Id { get; set; }

        [
            Required,
            StringLength(512, MinimumLength = 3)
        ]
        public string Title { get; set; }
        
        [
            Required,
            IsPositive(AllowZero = false)
        ]
        public long TemplateId { get; set; }
        
        [IsPositive(AllowZero = false)]
        public long? ApplicationId { get; set; }

        [
            Required,
            EnumDataType(typeof(NotificationType))
        ]
        public string Type { get; set; } = defaultType.ToString();
        
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
        public int Period { get; set; } = defaultPeriod;

        [
            Required,
            MinLength(1),
            MaxLength(10),
            ValidateListModels
        ]
        public ICollection<SetConditionRequest> Conditions { get; set; } = new List<SetConditionRequest>();

        [ArrayLength(10)]
        public long[] UserEmailIds { get; set; }
        
        public void Fill(NotificationRuleDto item)
        {
            Id = item?.Id;
            if (item != null)
            {
                Title = item.Title;
                TemplateId = item.MessageTemplate.Id;
                ApplicationId = item.Application?.Id;
                Type = item.Type.ToString();
                LogicOperator = item.LogicOperator.ToString();
                Period = item.Period;
                Conditions = item.Conditions.Select(condition => new SetConditionRequest(condition)).ToList();
                UserEmailIds = item.Emails.Select(item => item.Id).ToArray();
            }
        }
        
        public void Reset()
        {
            Id = null;
            Title = "";
            TemplateId = 0;
            ApplicationId = null;
            Type = defaultType.ToString();
            LogicOperator = string.Empty;
            Period = defaultPeriod;
            Conditions = new List<SetConditionRequest>();
        }
    }
}
