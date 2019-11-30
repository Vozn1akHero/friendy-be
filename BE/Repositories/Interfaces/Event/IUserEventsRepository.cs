using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IUserEventsRepository : IRepositoryBase<UserEvents>
    {
        Task<List<Event>> GetEventsByUserId(int userId);
        Task<List<EventDto>> GetShortenedEventsByUserId(int userId);
    }
}