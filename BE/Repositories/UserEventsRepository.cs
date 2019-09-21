using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class UserEventsRepository : RepositoryBase<UserEvents>, IUserEventsRepository
    {
        public UserEventsRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<List<UserEvents>> GetExampleEventsByUserId(int userId)
        {
            var exampleEventList = await FindByCondition(e => e.UserId == userId)
                .Include(e => e.Event)
                .Take(10)
                .ToListAsync();

            return exampleEventList;
        }
    }
}