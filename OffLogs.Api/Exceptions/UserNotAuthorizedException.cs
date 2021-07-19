using Domain.Abstractions;
using System;

namespace OffLogs.Api.Exceptions
{
    public class UserNotAuthorizedException: Exception, IDomainException
    {
        
    }
}