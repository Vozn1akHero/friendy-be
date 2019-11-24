using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IEventRepository : IRepositoryBase<Event>
    {
        Task<List<Event>> GetExampleEventsByUserId(int userId);
        Task<Event> GetById(int userId);
        Task<object> GetWithSelectedFields(int id, string[] selectedFields);
        Task<string> GetAvatarPathByEventIdAsync(int id);
        Task UpdateAvatarAsync(string path, int id);
        Task UpdateBackgroundAsync(string path, int id);
        //Task<byte[]> GetAvatarById(int id);
    }
}