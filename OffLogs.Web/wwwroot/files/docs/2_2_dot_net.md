## .Net Client

Для того что бы упростить отправку информации в OffLogs вы можете использовать уже готовую .Net
библиотеку которая предоставляет для этого удобный наборе методов.

### Установка

#### NuGet package manager:
```bash
Install-Package OffLogs.Client
```

### Использование

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

> Используете .Net Core? Посмотрите другую библиотеку [тут]() 
> которая адаптирована для этой платформы 
> 
> Looking for .NET Core? We have a quickstart for that too!
