CREATE TYPE LogPropertyType
    AS TABLE (
        Id bigint,
        LogId bigint,
        [Key] nvarchar(200),
        Value nvarchar(2048),
        CreateTime datetime2
    );

CREATE TYPE LogTraceType
    AS TABLE (
        Id bigint,
        LogId bigint,
        Trace nvarchar(2048),
        CreateTime datetime2
    );