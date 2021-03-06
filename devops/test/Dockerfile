FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY . ./
RUN echo "{}" > appsettings.Local.json
RUN echo "{}" > OffLogs.Api.Tests.Integration/appsettings.Local.json
RUN echo "{}" > OffLogs.Migrations/appsettings.Local.json

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
RUN dotnet restore

COPY ./devops/test/boot.sh /usr/local/bin/php_boot
CMD ["sh", "/usr/local/bin/php_boot"]