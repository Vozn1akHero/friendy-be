using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public interface IEventBannedUsersRepository : IRepositoryBase<EventBannedUsers>
    {
        Task<IEnumerable<EventBannedUsers>>
            FindEventBannedUsersByEventIdAsync(int eventId);

        void Add(EventBannedUsers eventBannedUsers);
        void RemoveByUserIdAndEventId(int id, int eventId);
        bool IsBannedAsync(int issuerId, int eventId);
    }
}