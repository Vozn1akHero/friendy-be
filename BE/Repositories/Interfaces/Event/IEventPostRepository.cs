using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces
{
    public interface IEventPostRepository : IRepositoryBase<EventPost>
    {
        Task CreateAsync(EventPost eventPost);
        Task<IEnumerable<EventPostOnWallDto>> GetRangeByIdAsync(int eventId, int startIndex, int length, int userId);
    }
}