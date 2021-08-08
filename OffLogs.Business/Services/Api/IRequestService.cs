using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Api
{
    public interface IRequestService
    {
        string GetApiToken();
        long GetUserIdFromJwt();
        long GetApplicationIdFromJwt();
    }
}
