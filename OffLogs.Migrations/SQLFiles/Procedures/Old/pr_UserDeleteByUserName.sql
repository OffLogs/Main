declare @proc varchar(100)
set @proc = 'pr_UserDeleteByUserName'
PRINT 'altering object "' + @proc + '":'
IF (OBJECT_ID(@proc) IS NULL) EXEC('CREATE PROCEDURE ' + @proc + ' as select ''stub''')
GO

ALTER PROCEDURE pr_UserDeleteByUserName
    @UserName nvarchar(MAX)
AS
    DELETE FROM [dbo].[Users] WHERE UserName = @UserName
GO    