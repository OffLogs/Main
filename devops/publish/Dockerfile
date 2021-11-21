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
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Assign permissions
RUN chmod -R 700 $KAFKA_HOME
RUN chmod -R 700 ./devops/common/kafka/boot.sh
RUN chmod -R 700 ./devops/common/zookeeper/boot.sh

# Create dirs
RUN 'mkdir /tmp/test'

ENV HOME = '/tmp'
ENV DOTNET_CLI_HOME = "/tmp/DOTNET_CLI_HOME"
ENV ASPNETCORE_ENVIRONMENT = "Development"
ENV DOTNET_USE_POLLING_FILE_WATCHER = true  
ENV ASPNETCORE_URLS = "http://+:80" 
RUN echo "{}" > appsettings.Local.json
RUN echo "{}" > OffLogs.Api.Tests.Integration/appsettings.Local.json
RUN echo "{}" > OffLogs.Migrations/appsettings.Local.json
RUN echo "{}" > OffLogs.Console/appsettings.Local.json
RUN dotnet restore
