using System;
using System.Collections.Generic;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class NotificationRuleDto : IResponse
    {
        public long Id { get; set; }
        
        public string Title { get; set; }
        
        public MessageTemplateDto MessageTemplate { get; set; }
        
        public ApplicationDto Application { get; set; }
        
        public NotificationType Type { get; set; }
        
        public LogicOperatorType LogicOperator { get; set; }
        
        public int Period { get; set; }
        
        public DateTime LastExecutionTime { get; set; }

        public ICollection<NotificationConditionDto> Conditions { get; set; } = new List<NotificationConditionDto>();
        
        public virtual DateTime CreateTime { get; set; }
        
        public virtual DateTime UpdateTime { get; set; }
    }
}
