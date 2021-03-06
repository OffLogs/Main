FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY . ./
RUN echo "{}" > appsettings.Local.json
RUN echo "{}" > OffLogs.Api.Tests.Integration/appsettings.Local.json
RUN echo "{}" > OffLogs.Migrations/appsettings.Local.json
RUN echo "{}" > OffLogs.Console/appsettings.Local.json
RUN dotnet restore

COPY ./devops/test/boot.sh /usr/local/bin/php_boot
CMD ["sh", "/usr/local/bin/php_boot"]