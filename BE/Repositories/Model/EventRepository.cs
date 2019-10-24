using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }


        public async Task<List<Event>> GetExampleEventsByUserId(int userId)
        {
/*
            var events = await FindByCondition(e => e.EventParticipants.Part)
                .Include(e => e.UserEvents)
                .ToListAsync();
            
            */

            return null;
        }

        public async Task<Event> GetById(int userId)
        {
            return await FindByCondition(e => e.Id == userId).SingleOrDefaultAsync();
        }
    }
}