using System;
using System.Collections.Generic;
using Api.Requests.Abstractions;
using OffLogs.Business.Common.Constants.Notificatiions;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class NotificationRuleDto : IResponse
    {
        public long Id { get; set; }
        
        public NotificationMessageDto Message { get; set; }
        
        public NotificationType Type { get; set; }
        
        public LogicOperatorType LogicOperator { get; set; }
        
        public long Period { get; set; }
        
        public DateTime LastExecutionTime { get; set; }
        
        public ICollection<NotificationConditionDto> Conditions { get; set; }
        
        public virtual DateTime CreateTime { get; set; }
        
        public virtual DateTime UpdateTime { get; set; }
    }
}
