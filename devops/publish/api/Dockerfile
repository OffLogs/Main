FROM mcr.microsoft.com/dotnet/sdk:6.0

# Change timezone to local time
ENV TZ=UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

COPY . /app
RUN ls /app
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80

RUN apt update
RUN apt install -y apt-transport-https wget ca-certificates

RUN echo "{}" > ./OffLogs.Api/appsettings.Local.json
RUN echo "{}" > ./OffLogs.Api.Tests.Integration/appsettings.Local.json
RUN echo "{}" > ./OffLogs.Console/appsettings.Local.json

RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]

COPY ./devops/publish/api/boot.sh /usr/local/bin/php_boot

CMD ["sh", "/usr/local/bin/php_boot"]
