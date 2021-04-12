declare @proc varchar(100)
set @proc = 'pr_ApplicationGet'
PRINT 'altering object "' + @proc + '":'
IF (OBJECT_ID(@proc) IS NULL) EXEC('CREATE PROCEDURE ' + @proc + ' as select ''stub''')
GO

ALTER PROCEDURE pr_ApplicationGet
    @Id bigint
AS
    SELECT TOP 1 * FROM [dbo].[Applications] WHERE Id = @Id
GO    