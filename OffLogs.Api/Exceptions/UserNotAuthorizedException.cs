using System;
using Domain.Abstractions;

namespace OffLogs.Api.Exceptions
{
    public class UserNotAuthorizedException : Exception, IDomainException
    {

    }
}