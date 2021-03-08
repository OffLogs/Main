#!/bin/bash

>&2 echo "Connection string: $ConnectionStrings__DefaultConnection"

>&2 echo "Run Migrations"
dotnet run --configuration Development