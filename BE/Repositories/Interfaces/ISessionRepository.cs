using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using Microsoft.AspNetCore.Mvc;

namespace BE.Interfaces
{
    public interface ISessionRepository : IRepositoryBase<Session>
    {
        Task<Session> GetSession(string hash);
        Task<Session> CreateSession(string token);
        Task UpdateSession(Session session);
        Task DeleteSession(Session session);
    }
}
