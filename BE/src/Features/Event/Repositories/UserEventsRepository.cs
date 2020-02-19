using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Event.Repositories
{
    public class UserEventsRepository : RepositoryBase<UserEvents>, IUserEventsRepository
    {
        public UserEventsRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<List<Models.Event>> GetEventsByUserId(int userId)
        {
            return await FindByCondition(e => e.UserId == userId)
                .Select(e => e.Event)
                .ToListAsync();
        }

        /*public async Task<List<Models.Event>> GetParticipatingByUserId(int userId)
        {
            var events = await FindByCondition(e => e.UserId == userId)
                .ToListAsync();
            return events;
        }*/
    }
}