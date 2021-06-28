using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OffLogs.Business.Common.Models.Api.Response.Board;
using OffLogs.Business.Constants;
using OffLogs.Business.Extensions;
using OffLogs.Business.Helpers;

namespace OffLogs.Business.Db.Entity
{
    public class RequestLogEntity
    {
        public virtual long Id { get; set; }

        public virtual RequestLogType Type { get; set; }
        
        public virtual string Token { get; set; }
        
        public virtual string ClientIp { get; set; }
        
        public virtual string Data { get; set; }
        
        public virtual DateTime CreateTime { get; set; }
    }
}