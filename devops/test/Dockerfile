FROM ubuntu:focal
WORKDIR /app
USER root

RUN cat /etc/os-release

COPY . /app
RUN mkdir /tmp/test

RUN apt update
RUN apt install -y apt-transport-https wget ca-certificates iputils-ping telnet curl gnupg2

# Configure time
ARG DEBIAN_FRONTEND=noninteractive
ENV TZ = "UTC"
RUN apt-get install -y tzdata
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# Fix error: The configured user limit (128) on the number of inotify instances has been reached
#RUN echo "fs.inotify.max_user_watches=524288" >> /etc/sysctl.conf
#RUN echo "fs.inotify.max_user_instances=524288" >> /etc/sysctl.conf
#RUN sysctl -p

# Install .Net Core 5.0
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
RUN curl https://packages.microsoft.com/config/ubuntu/20.10/prod.list | tee /etc/apt/sources.list.d/msprod.list
RUN apt update
RUN apt install -y unixodbc-dev dotnet-sdk-5.0

# Update
RUN apt-get install -y net-tools sudo netcat \
    default-jre \
    postgresql postgresql-client

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

# Env vars
# Zookeeper
ENV ZOOKEEPER_CLIENT_PORT = 2181
ENV ZOOKEEPER_TICK_TIME = 2000

# Kafka
ENV KAFKA_HOME = "./devops/common/binable/kafka"
ENV KAFKA_BROKER_ID = 1
ENV KAFKA_ZOOKEEPER_CONNECT = "localhost:2181"
ENV KAFKA_LISTENERS = "INSIDE://:9092,OUTSIDE://:9094"
ENV KAFKA_ADVERTISED_LISTENERS = "INSIDE://:9092,OUTSIDE://localhost:9094"
ENV KAFKA_LISTENER_SECURITY_PROTOCOL_MAP = "INSIDE:PLAINTEXT,OUTSIDE:PLAINTEXT"
ENV KAFKA_INTER_BROKER_LISTENER_NAME = "INSIDE"
ENV PATH = "$PATH:$KAFKA_HOME/bin"

# Postgres
ENV POSTGRES_CONNECTION_RETRIES = 5

ENTRYPOINT ["tail", "-f", "/dev/null"]
