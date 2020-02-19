using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public interface IEventPostRepository : IRepositoryBase<EventPost>
    {
        Task CreateAsync(EventPost eventPost);
        Task<EventPost> GetByIdAsync(int id);

        Task<IEnumerable<EventPost>> GetRangeByIdAsync(int eventId, int startIndex,
            int length);

        Task<IEnumerable<EventPost>> GetLastRangeByIdAsync(int eventId, int lastIndex,
            int length);

        Task<IEnumerable<EventPost>> GetLastRangeByIdWithPaginationAsync(int eventId,
            int page);
    }
}