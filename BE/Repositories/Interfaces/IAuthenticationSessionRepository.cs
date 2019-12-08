using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using Microsoft.AspNetCore.Mvc;

namespace BE.Interfaces
{
    public interface IAuthenticationSessionRepository : IRepositoryBase<AuthenticationSession>
    {
        Task<AuthenticationSession> GetSession(string hash);
        Task<AuthenticationSession> CreateAndReturn(string token);
        Task RefreshTokenByToken(string previousToken, string newToken);
        Task DeleteSession(AuthenticationSession session);
    }
}
