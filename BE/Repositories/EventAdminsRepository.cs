using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class EventAdminsRepository : RepositoryBase<EventAdmins>, IEventAdminsRepository
    {
        public EventAdminsRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }
        
        public async Task<List<Event>> GetUserAdministeredEvents(int userId)
        {
            return await FindByCondition(e => e.UserId == userId)
                .Select(e => e.Event)
                .ToListAsync();
        }  
        
        public async Task<List<Event>> FilterAdministeredEvents(int userId, string keyword)
        {
            return await FindByCondition(e => e.UserId == userId && e.Event.Title.Contains(keyword))
                .Select(e => e.Event)
                .ToListAsync();
        }
    }
}