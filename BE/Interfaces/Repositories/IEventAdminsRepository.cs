using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IEventAdminsRepository: IRepositoryBase<EventAdmins>
    {
        Task<List<Event>> GetUserAdministeredEvents(int userId);
    }
}