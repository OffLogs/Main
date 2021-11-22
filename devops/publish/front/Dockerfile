FROM mcr.microsoft.com/dotnet/sdk:5.0
#USER root

COPY . /app
WORKDIR /app

# Update
RUN apt-get update --fix-missing
RUN apt-get install -y apt-transport-https wget ca-certificates apt-transport-https debconf-utils \
    net-tools sudo netcat

# Configure time
ENV TZ = "UTC"
#RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

ENV ASPNETCORE_ENVIRONMENT = "Development"
ENV DOTNET_USE_POLLING_FILE_WATCHER = true  
ENV ASPNETCORE_URLS = "http://+:80" 

RUN ls
RUN echo "{}" > OffLogs.Api.Frontend/appsettings.Local.json
RUN dotnet build OffLogs.Api.Frontend/

COPY ./devops/publish/front/boot.sh /usr/local/bin/php_boot
CMD ["sh", "/usr/local/bin/php_boot"]
