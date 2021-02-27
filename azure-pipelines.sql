IF EXISTS(SELECT * FROM sys.databases WHERE name = 'OffLogs')
    BEGIN
        DROP DATABASE [OffLogs]
    END

CREATE DATABASE [OffLogs]