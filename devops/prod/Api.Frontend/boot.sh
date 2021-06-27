#!/bin/bash

>&2 echo "SQL Server is up - executing command"
cd ../OffLogs.Api.Frontend
dotnet run --no-launch-profile