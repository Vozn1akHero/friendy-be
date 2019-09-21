using System.Collections.Generic;
using System.Security.Claims;

namespace BE.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwt(List<Claim> claims);
        bool ValidateJwt(string token);
        int GetUserIdFromJwt(string token);
    }
}