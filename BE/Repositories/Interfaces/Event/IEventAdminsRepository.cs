using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Dtos.EventDtos;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IEventAdminsRepository: IRepositoryBase<EventAdmins>
    {
        Task<List<Event>> GetUserAdministeredEvents(int userId);
        Task<List<EventAdminDto>> GetByEventIdAsync(int eventId);
        Task DeleteByEventIdAndAdminId(int eventId, int adminId);
        Task<List<EventDto>> GetShortenedAdministeredEventsByUserId(int userId);
        Task<List<Event>> FilterAdministeredEvents(int userId, string keyword);
        Task<bool> IsUserAdminById(int eventId, int userId);
    }
}