declare @proc varchar(100)
set @proc = 'AddDeviceRequestGetByToken'
PRINT 'altering object "' + @proc + '":'
IF (OBJECT_ID(@proc) IS NULL) EXEC('CREATE PROCEDURE ' + @proc + ' as select ''stub''')
EXEC('GRANT EXEC ON  ' + @proc + ' TO QRIDAppRole') -- run this every time, so this script could also be used for restoring missed GRANTS
GO

alter procedure AddDeviceRequestGetByToken
@QrCodeToken nvarchar(1000)
as

SELECT *
	FROM [dbo].[AddDeviceRequest] 
	WHERE QrCodeToken = @QrCodeToken

GO    