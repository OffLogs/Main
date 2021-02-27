FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

RUN echo "{}" > appsettings.Local.json

COPY . ./
RUN dotnet restore

COPY . ./
#ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.7.3/wait /wait
ADD https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

ENV ConnectionStrings__DefaultConnection: 'Data Source=database;Initial Catalog=master;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!'
CMD /wait-for-it.sh database:1433 -t 120 
CMD dotnet test --logger trx --results-directory /var/temp OffLogs.Api.Tests.Integration/

#CMD /wait
#CMD dotnet test --logger trx --results-directory /var/temp OffLogs.Api.Tests.Integration/