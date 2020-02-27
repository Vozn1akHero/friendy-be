using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Event.Repositories
{
    public class EventBannedUsersRepository : RepositoryBase<EventBannedUsers>,
        IEventBannedUsersRepository
    {
        public EventBannedUsersRepository(FriendyContext friendyContext) : base(
            friendyContext)
        {
        }

        public async Task<IEnumerable<EventBannedUsers>>
            FindEventBannedUsersByEventIdAsync(int eventId)
        {
            return await FindByCondition(e => e.EventId == eventId)
                .Include(e => e.User)
                .ToListAsync();
        }

        public void Add(EventBannedUsers eventBannedUsers)
        {
            Create(eventBannedUsers);
        }

        public void RemoveByUserIdAndEventId(int id, int eventId)
        {
            var entity = FindByCondition(e => e.UserId == id && e.EventId ==
                                              eventId).SingleOrDefault();
            Delete(entity);
        }

        public bool IsBannedAsync(int issuerId, int eventId)
        {
            return ExistsByCondition(e => e.EventId == eventId && e.UserId == issuerId);
        }
    }
}