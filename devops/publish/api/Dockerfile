FROM mcr.microsoft.com/dotnet/sdk:6.0

# Change timezone to local time
ENV TZ=UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

COPY . /app
RUN ls /app
WORKDIR /app/OffLogs.Api

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80

RUN echo "{}" > appsettings.Local.json

COPY ./devops/publish/api/boot.sh /usr/local/bin/php_boot

CMD ["sh", "/usr/local/bin/php_boot"]
