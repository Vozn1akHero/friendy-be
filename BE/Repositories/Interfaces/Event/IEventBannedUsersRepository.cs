using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces
{
    public interface IEventBannedUsersRepository : IRepositoryBase<EventBannedUsers>
    {
        Task<IEnumerable<EventBannedUsers>>
            FindEventBannedUsersByEventIdAsync(int eventId);

        void Add(EventBannedUsers eventBannedUsers);
        void RemoveByUserIdAndEventId(int id, int eventId);
    }
}