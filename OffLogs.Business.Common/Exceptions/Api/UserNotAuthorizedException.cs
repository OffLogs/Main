using System;
using Domain.Abstractions;

namespace OffLogs.Business.Common.Exceptions.Api
{
    public class UserNotAuthorizedException : Exception, IDomainException
    {

    }
}
