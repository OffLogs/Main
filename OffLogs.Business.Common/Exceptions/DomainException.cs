using OffLogs.Business.Common.Exceptions.Api;
using OffLogs.Business.Common.Exceptions.Common;
using ValidationException = OffLogs.Business.Common.Exceptions.Common.ValidationException;

namespace OffLogs.Business.Common.Exceptions;

public static class DomainException
{
    public static DataPermissionException DataPermissionException { get; } = new();
    
    public static ItemNotFoundException ItemNotFoundException { get; } = new();
    
    public static PermissionException PermissionException { get; } = new();
    
    public static TooManyRequestsException TooManyRequestsException { get; } = new();

    public static ValidationException ValidationException { get; } = new();
    
    public static RecordIsExistsException RecordIsExistsException { get; } = new();
    
    public static RecordNotFoundException RecordNotFoundException { get; } = new();
    
    public static ServerException ServerException { get; } = new();
    
    public static TooManyRecordsException TooManyRecordsException { get; } = new();
    
    public static UserNotAuthorizedException UserNotAuthorizedException { get; } = new();
}
