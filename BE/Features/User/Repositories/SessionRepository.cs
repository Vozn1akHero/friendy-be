using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.User.Repositories
{
    public class SessionRepository : RepositoryBase<Session>, ISessionRepository
    {
        public SessionRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<Session> CreateAndReturn(Session session)
        {
            Create(session);
            await SaveAsync();
            return session;
        }

        /*public async Task<Session> GetByUserId(int id)
        {
            return await FindByCondition(e => e.UserId == id)
                .SingleOrDefaultAsync();
        }*/

        public async Task<Session> GetByConnectionId(string id)
        {
            return await FindByCondition(e => e.ConnectionId == id)
                .SingleOrDefaultAsync();
        }

        public async Task<Session> UpdateAndReturn(Session session)
        {
            Update(session);
            await SaveAsync();
            return session;
        }
    }
}