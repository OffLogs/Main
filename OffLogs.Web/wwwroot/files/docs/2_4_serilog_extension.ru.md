## Расширение для Serilog

Если в своем проекте вы используете Serilog, 
тогда вы можете добавить Sink от OffLogs.
Это расширение использует OffLogs.Client для отправки информации в OffLogsApi, 
а так же поддерживает некоторые дополнительные опции

### Установка

Установить его вы можете из NuGet:

```bash
Install-Package Newtonsoft.Json
Install-Package OffLogs.Client
Install-Package Serilog.Sinks.OffLogs
```

### Настройка

> Процесс установки Serilog можно изучить на
> [официальном сайте](https://serilog.net/)
> 
> В текущем примере показано как настроить клиент OffLogs для 
> [Serilog.AspNetCore](https://github.com/serilog/serilog-aspnetcore).

Ниже приведен пример кода который включает возможности клиента OffLogs для Serilog:

```c#
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.OffLogs("Api Token");
});
```

> Тестовый проект вы можете скачать и опробовать из нашего репозитория на
> [GitLab](https://gitlab.com/offlogs-public/client-dotnet/-/tree/dev/Serilog.Sinks.OffLogs.Example)

> Информацию о том как можно получить API токен
> можно прочитать [тут](/documentation/common/1_3_applications)
