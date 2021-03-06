declare @proc varchar(100)
set @proc = 'AddDeviceRequestGetByToken'
PRINT 'altering object "' + @proc + '":'
IF (OBJECT_ID(@proc) IS NULL) EXEC('CREATE PROCEDURE ' + @proc + ' as select ''stub''')
GO

alter procedure AddDeviceRequestGetByToken
@QrCodeToken nvarchar(1000)
as

SELECT *
	FROM [dbo].[AddDeviceRequest] 
	WHERE QrCodeToken = @QrCodeToken

GO    