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
    public class AuthenticationSessionRepository : RepositoryBase<AuthenticationSession>, IAuthenticationSessionRepository
    {
        public AuthenticationSessionRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public Task<AuthenticationSession> GetSession(string hash)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationSession> CreateAndReturn(string token)
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
                var session = new AuthenticationSession()
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

        public async Task RefreshTokenByToken(string previousToken, string newToken)
        {
            var session = await FindByCondition(e => e.Token == previousToken).SingleOrDefaultAsync();
            session.Token = newToken;
            await SaveAsync();
        }

        public Task DeleteSession(AuthenticationSession session)
        {
            throw new NotImplementedException();
        }
    }
}
