#!/bin/bash

#until dotnet ef database update; do
#    >&2 echo "SQL Server is starting up"
#    sleep 1
#done

>&2 echo "Run Migrations"
cd ../OffLogs.Migrations
dotnet run --configuration Development

>&2 echo "SQL Server is up - executing command"
cd ../OffLogs.Api
dotnet run --no-launch-profile