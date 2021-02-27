IF EXISTS(SELECT * FROM sys.databases WHERE name = 'ViberBot')
    BEGIN
        DROP DATABASE [ViberBot]
    END

CREATE DATABASE [ViberBot]

IF EXISTS(SELECT * FROM sys.databases WHERE name = 'Intercome system')
    BEGIN
        DROP DATABASE [Intercome system]
    END

CREATE DATABASE [Intercome system]

IF EXISTS(SELECT * FROM sys.databases WHERE name = 'IntercomeSystemMobile')
    BEGIN
        DROP DATABASE [IntercomeSystemMobile]
    END

CREATE DATABASE [IntercomeSystemMobile]