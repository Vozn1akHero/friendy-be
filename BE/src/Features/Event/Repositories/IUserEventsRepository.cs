using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Event.Repositories
{
    public interface IUserEventsRepository : IRepositoryBase<UserEvents>
    {
        Task<List<Models.Event>> GetEventsByUserId(int userId);
        //Task<List<Event>> GetParticipatingByUserId(int userId);
    }
}