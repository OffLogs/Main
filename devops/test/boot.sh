#!/bin/bash

>&2 echo "Connection string: $ConnectionStrings__DefaultConnection"

>&2 echo "Wait for DB"
sleep 10

>&2 echo "Run Migrations"
cd OffLogs.Migrations/
dotnet run --configuration Development

>&2 echo "Run Tests"
cd ../OffLogs.Api.Tests.Integration
dotnet test --logger trx --results-directory /var/temp .