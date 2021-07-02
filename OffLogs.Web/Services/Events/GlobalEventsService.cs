using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Web.Services.Events
{
    public class GlobalEventsService: IGlobalEventsService
    {
        public event Action OnClickDocument;
        public Task InvokeOnClickDocumentAsync()
        {
            return Task.Run(() =>
            {
                OnClickDocument?.Invoke();
            });
        }
    }
}
