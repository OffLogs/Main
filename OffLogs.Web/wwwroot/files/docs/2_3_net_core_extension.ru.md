## .Net Core Client

Мы рекомендуем использовать это расширение если вы используете .Net Core.

### Установка

#### NuGet package manager
```bash
Install-Package OffLogs.Client
Install-Package OffLogs.Client.AspNetCore
```

### Использование

#### 1. В файле Program.cs добавьте инициализацию нового логера

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


#### 2. Добавьте необходимую конфигурацию библиотеки в appsettings.json

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
> Этот конфигурационный параметр отвечает за то данные каких
> логов будут отправлены в OffLogs.
> 
> Может содержать следующие значения:
> - Trace
> - Debug
> - Information
> - Warning
> - Error
> - Critical
> 
> Это означает, что если **MinLogLevel** будет содержать значение **Information** тогда информация
> о логах с типом **Trace** и **Debug** отправлены не будут
