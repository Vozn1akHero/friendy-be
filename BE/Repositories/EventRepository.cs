using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;

namespace BE.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }


        public Task<List<Event>> GetExampleEventsByUserId(int userId)
        {
            //var events = FindByCondition(e => e.)
            return null;
        }
    }
}