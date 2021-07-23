using System;
using Domain.Abstractions;

namespace OffLogs.Api.Business.Exceptions
{
    public class UserNotAuthorizedException: Exception, IDomainException
    {
        
    }
}