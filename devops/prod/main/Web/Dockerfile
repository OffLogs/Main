FROM mcr.microsoft.com/dotnet/sdk:5.0

# Change timezone to local time
ENV TZ=UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

COPY . /app
RUN ls /app
WORKDIR /app/OffLogs.Web

# Install nginx
RUN apt-get update --fix-missing
RUN apt-get install -y nginx
RUN rm /etc/nginx/sites-available/default
RUN echo "daemon off;" >> /etc/nginx/nginx.conf
COPY ./prod/Web/nginx.conf /etc/nginx/sites-available/default

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80

RUN apt update
RUN apt install -y apt-transport-https wget ca-certificates
RUN echo "{}" > appsettings.Local.json
RUN dotnet publish --configuration=Production -o "/var/www" -f net5.0 -r linux-x64 --self-contained true
RUN ls /var/www/wwwroot

COPY ./prod/Web/boot.sh /usr/local/bin/php_boot
CMD ["sh", "/usr/local/bin/php_boot"]