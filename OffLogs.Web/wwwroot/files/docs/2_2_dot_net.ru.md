## .Net Client

In order to simplify sending information to OffLogs, you can use ready-made .Net
a library that provides a convenient set of methods for this.

### Installation

#### NuGet package manager:
```bash
Install-Package OffLogs.Client
```

### Usage

```c#
using OffLogs.Client;
using Microsoft.Extensions.Logging;

namespace TestClient
{
    public class TestClientClass
    {
        public static void Main(string[] args)
        {
            // 1. Init application token
            var apiToken = "<Application API Token>";

            // 2. Set up Offlogs client
            var client = new OffLogsHttpClient();
            client.SetApiToken(apiToken);
            
            // 3. Send log
            client.SendLog(LogLevel.Debug, "Some message text");           
        }
    }
}
```

> Looking for .Net Core? We have a quickstart for that [too!](/documentation/common/2_4_serilog_extension)
