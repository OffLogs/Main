FROM mcr.microsoft.com/dotnet/sdk:5.0

# Change timezone to local time
ENV TZ=UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

COPY . /app
RUN ls /app
WORKDIR /app/Vizit.Api

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80

RUN apt update
RUN apt install -y apt-transport-https wget ca-certificates

RUN echo "{}" > appsettings.Local.json
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]

COPY ./Vizit.Api/devops/dev/boot.sh /usr/local/bin/php_boot

CMD ["sh", "/usr/local/bin/php_boot"]