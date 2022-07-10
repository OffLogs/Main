## Serilog Extension

If you are using Serilog in your project,
then you can add Sink from OffLogs.
This extension uses OffLogs.Client to send information to OffLogsApi,
and also supports some additional options

### Installation

You can install it from NuGet:

```bash
Install-Package Newtonsoft.Json
Install-Package OffLogs.Client
Install-Package Serilog.Sinks.OffLogs
```

### Configuring

> See the Serilog installation process at
> [official website](https://serilog.net/)
> 
> The current example shows how to configure the OffLogs client for
> [Serilog.AspNetCore](https://github.com/serilog/serilog-aspnetcore).

The following code example includes the capabilities of the OffLogs client for Serilog:

```c#
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.OffLogs("Api Token");
});
```

> You can download and test the test project from our repository on
> [GitLab](https://gitlab.com/offlogs-public/client-dotnet/-/tree/dev/Serilog.Sinks.OffLogs.Example)

> With information on how to get an API token
> can be found [here](/documentation/common/1_3_applications)
