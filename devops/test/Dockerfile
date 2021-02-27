FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

RUN echo "{}" > appsettings.Local.json

COPY . ./
RUN dotnet restore

COPY . ./
CMD dotnet test --logger trx --results-directory /var/temp OffLogs.Api.Tests.Integration/