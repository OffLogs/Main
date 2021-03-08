declare @proc varchar(100)
set @proc = 'pr_LogGetList'
PRINT 'altering object "' + @proc + '":'
IF (OBJECT_ID(@proc) IS NULL) EXEC('CREATE PROCEDURE ' + @proc + ' as select ''stub''')
GO

ALTER PROCEDURE pr_LogGetList
    @ApplicationId bigint,
    @Page integer,
    @PageSize integer = 30
AS
    SET NOCOUNT ON;

    SET @Page = @Page - 1
    SET @Page = IIF(@Page <= 0, 0, @Page)
    DECLARE @Offset bigint = @Page * @PageSize
    
    SELECT
        logT.*,
        lp.*,
        lt.*,
        (
            SELECT COUNT(Id) FROM [Logs] WHERE ApplicationId = @ApplicationId    
        ) AS sumCount
    FROM (
        SELECT * FROM [Logs]
            WHERE ApplicationId = @ApplicationId
            ORDER BY CreateTime DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY               
    ) AS logT
    LEFT JOIN LogProperties AS lp ON lp.LogId =  logT.Id
    LEFT JOIN LogTraces AS lt ON lt.LogId =  logT.Id
GO    