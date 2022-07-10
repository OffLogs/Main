## .Net Core Client

We recommend using this extension if you are using .Net Core.

### Installation

#### NuGet package manager
```bash
Install-Package OffLogs.Client
Install-Package OffLogs.Client.AspNetCore
```

### Usage

#### 1. In the Program.cs file, add the initialization of the new logger

```c#
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OffLogs.Client.AspNetCore;


namespace TestNetCoreClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => {
                    // OffLogs logger initialization
                    logging.AddOffLogsLogger();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
```


#### 2. Add required library configuration to appsettings.json

```json
{
    "OffLogs": {
        "ApiToken": "<Application API token>",
        "MinLogLevel": "Trace"
    }
}
```

> MinLogLevel
>
> This configuration parameter controls which log data will be sent to OffLogs.
>
> May contain the following values:
> - Trace
> - Debug
> - Information
> - Warning
> - Error
> - Critical
>
> This means that if **MinLogLevel** contains the value **Information** 
> then information about logs with type **Trace** and **Debug** will not be sent
