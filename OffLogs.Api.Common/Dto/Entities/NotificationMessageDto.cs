﻿using Api.Requests.Abstractions;

namespace OffLogs.Api.Common.Dto.Entities
{
    public class NotificationMessageDto : IResponse
    {
        public long Id { get; set; }
        
        public string Subject { get; set; }
        
        public string Body { get; set; }
    }
}
