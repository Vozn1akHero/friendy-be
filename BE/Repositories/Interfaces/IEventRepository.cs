using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IEventRepository : IRepositoryBase<Event>
    {
        Task<List<Event>> GetExampleEventsByUserId(int userId);
        Task<Event> GetById(int userId);
    }
}