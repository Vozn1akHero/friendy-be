using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BE.Repositories
{
    public class SessionRepository : RepositoryBase<Session>, ISessionRepository
    {
        public SessionRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public Task<Session> GetSession(string hash)
        {
            throw new NotImplementedException();
        }

        public async Task<Session> CreateSession(string token)
        {
            var bytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            string hash = BitConverter.ToString(bytes);

            var existingSession = await FindByCondition(e => e.Token == token)
                .SingleOrDefaultAsync();

            if (existingSession == null)
            {
                var session = new Session()
                {
                    Hash = hash,
                    Token = token
                };

                Create(session);
                await SaveAsync();

                return session;
            }

            return existingSession;
        }

        public Task UpdateSession(Session session)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSession(Session session)
        {
            throw new NotImplementedException();
        }
    }
}
