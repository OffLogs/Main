using System;
using Domain.Abstractions;
using OffLogs.Business.Orm.Entities;

namespace OffLogs.Business.Services.Jwt
{
    public interface IJwtApplicationService: IDomainService
    {
        public string BuildJwt(ApplicationEntity application);
        public long? GetApplicationId(string jwtString);
        long? GetUserId(string jwtString);
        byte[]? GetApplicationPublicKey(string jwtString);
        public bool IsValidJwt(string jwtString);
    }
}
