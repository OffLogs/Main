IF OBJECT_ID (N'dbo.fn_ApplicationIsOwner', N'FN') IS NOT NULL
    DROP FUNCTION fn_ApplicationIsOwner;
GO

CREATE FUNCTION fn_ApplicationIsOwner(@UserId bigint, @ApplicationId bigint)
    RETURNS BIT
AS
BEGIN
    DECLARE @ExistId int
    SELECT @ExistId = COUNT(Id) FROM [dbo].[Applications] WHERE UserId = @UserId AND Id = @ApplicationId
    IF @ExistId IS NULL
        BEGIN
            SET @ExistId = 0;    
        END
    RETURN @ExistId
END
GO    