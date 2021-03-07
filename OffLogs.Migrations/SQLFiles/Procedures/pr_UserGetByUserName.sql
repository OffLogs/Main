declare @proc varchar(100)
set @proc = 'pr_UserGetByUserName'
PRINT 'altering object "' + @proc + '":'
IF (OBJECT_ID(@proc) IS NULL) EXEC('CREATE PROCEDURE ' + @proc + ' as select ''stub''')
GO

ALTER PROCEDURE pr_UserGetByUserName
    @UserName nvarchar(MAX)
AS
    SELECT TOP 1 * FROM [dbo].[Users] WHERE UserName = @UserName
GO    