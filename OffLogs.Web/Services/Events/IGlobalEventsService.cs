using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffLogs.Web.Services.Events
{
    public interface IGlobalEventsService
    {
        event Action OnClickDocument;

        Task InvokeOnClickDocumentAsync();
    }
}
