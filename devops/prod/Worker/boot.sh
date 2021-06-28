#!/bin/bash

>&2 echo "SQL Server is up - executing command"
cd ../OffLogs.Api.Worker
dotnet run --no-launch-profile