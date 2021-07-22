using System;
using Domain.Abstractions;

namespace Offlogs.Business.Api.Exceptions
{
    public class UserNotAuthorizedException: Exception, IDomainException
    {
        
    }
}