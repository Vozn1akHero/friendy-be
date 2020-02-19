using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Features.Event.Dto;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public interface IEventAdminsRepository : IRepositoryBase<EventAdmins>
    {
        Task CreateAndReturn(int eventId, int userId);
        Task<List<EventAdminDto>> GetByEventIdAsync(int eventId);
        Task DeleteByEventIdAndAdminId(int eventId, int adminId);
        Task<List<EventDto>> GetShortenedAdministeredEventsByUserId(int userId);
        Task<List<Models.Event>> FilterAdministeredEvents(int userId, string keyword);
        Task<bool> IsUserAdminById(int eventId, int userId);
    }
}