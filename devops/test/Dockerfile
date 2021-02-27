FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

RUN echo "{}" > appsettings.Local.json

COPY . ./
RUN dotnet restore

COPY . ./
#ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.7.3/wait /wait
#RUN chmod +x /wait

ENV ConnectionStrings__DefaultConnection: 'Data Source=database;Initial Catalog=master;PersistSecurityInfo=True;User ID=sa;Password=P@ssW0rd!'
#CMD /wait
CMD dotnet test --logger trx --results-directory /var/temp OffLogs.Api.Tests.Integration/