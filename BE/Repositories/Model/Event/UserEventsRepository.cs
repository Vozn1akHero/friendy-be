using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Dtos.FriendDtos;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class UserEventsRepository : RepositoryBase<UserEvents>, IUserEventsRepository
    {
        public UserEventsRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<List<Models.Event>> GetEventsByUserId(int userId)
        {
            return await FindByCondition(e => e.UserId == userId)
                .Select(e => e.Event)
                .ToListAsync();
        }
        
        /*public async Task<List<Models.Event>> GetParticipatingByUserId(int userId)
        {
            var events = await FindByCondition(e => e.UserId == userId)
                .ToListAsync();
            return events;
        }*/
    }
}