FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80

RUN apt update
RUN apt install -y apt-transport-https wget ca-certificates

RUN echo "{}" > appsettings.Local.json

COPY . ./
RUN dotnet restore

COPY . ./
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.5.0/wait /wait
RUN chmod +x /wait

ENV ConnectionStrings__DefaultConnection: 'Data Source=database;Initial Catalog=master;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!'
CMD /wait && dotnet test --logger trx --results-directory /var/temp OffLogs.Api.Tests.Integration/OffLogs.Api.Tests.Integration.csproj