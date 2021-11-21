FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app
USER root

COPY . /app

# Update
RUN apt-get update
RUN apt-get install -y apt-transport-https wget ca-certificates apt-transport-https debconf-utils \
    net-tools sudo netcat

# Configure time
ENV TZ = "UTC"
#RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

#ENV HOME = '/tmp'
#ENV DOTNET_CLI_HOME = "/tmp/DOTNET_CLI_HOME"

ENV ASPNETCORE_ENVIRONMENT = "Development"
ENV DOTNET_USE_POLLING_FILE_WATCHER = true  
ENV ASPNETCORE_URLS = "http://+:80" 

RUN ls
RUN echo "{}" > OffLogs.Api/appsettings.Local.json
#RUN echo "{}" > OffLogs.Migrations/appsettings.Local.json
#RUN echo "{}" > OffLogs.Console/appsettings.Local.json
RUN dotnet restore
