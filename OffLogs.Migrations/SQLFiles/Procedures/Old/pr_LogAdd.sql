declare @proc varchar(100)
set @proc = 'pr_LogAdd'
PRINT 'altering object "' + @proc + '":'
IF (OBJECT_ID(@proc) IS NULL) EXEC('CREATE PROCEDURE ' + @proc + ' as select ''stub''')
GO

ALTER PROCEDURE pr_LogAdd
    @ApplicationId bigint,
    @Message nvarchar(2048),
    @Level nvarchar(20),
    @Timestamp DATETIME2,
    @Properties LogPropertyType READONLY,
    @Traces LogTraceType READONLY
AS
    SET NOCOUNT ON;
    
    INSERT INTO [Logs] (
        ApplicationId,
        [Level],
        [Message],
        LogTime,
        CreateTime
    ) VALUES (
        @ApplicationId,
        @Level,
        @Message,
        @Timestamp,
        GETDATE()
    )
    DECLARE @LogId bigint = SCOPE_IDENTITY();

    INSERT INTO [LogProperties] (
        LogId,
        [Key],
        [Value],
        CreateTime
    ) 
        SELECT 
            @LogId AS LogId,
            t1.[Key],
            t1.[Value],
            t1.CreateTime
        FROM @Properties AS t1

    INSERT INTO [LogTraces] (
        LogId,
        [Trace],
        CreateTime
    )
        SELECT
            @LogId AS LogId,
            t1.[Trace],
            t1.CreateTime
        FROM @Traces AS t1
GO    