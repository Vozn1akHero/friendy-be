using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IEventRepository : IRepositoryBase<Event>
    {
        Task<Event> CreateAndReturn(Event @event);
        Task<Event> GetById(int userId);
        Task<IEnumerable<Event>> SearchByKeyword(string keyword);
        Task<bool> IsUserCreatorById(int id, int userId);
        Task<object> GetWithSelectedFields(int id, string[] selectedFields);
        Task<IEnumerable<Event>> FilterParticipatingByKeywordAndUserId(int userId, string keyword);
        Task<IEnumerable<Event>> FilterAdministeredByKeywordAndUserId(int userId, string keyword);
        Task<IEnumerable<Event>> GetParticipatingByUserIdAsync(int userId);
        Task<IEnumerable<Models.Event>> GetAdministeredByUserIdAsync(int userId);
        Task<IEnumerable<Models.Event>> GetClosestAsync(string city, int length);
    }
}