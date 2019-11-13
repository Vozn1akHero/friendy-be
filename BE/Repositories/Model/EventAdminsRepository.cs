using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class EventAdminsRepository : RepositoryBase<EventAdmins>, IEventAdminsRepository
    {
        public EventAdminsRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }
        
        public async Task<List<Event>> GetUserAdministeredEvents(int userId)
        {
            return await FindByCondition(e => e.UserId == userId)
                .Select(e => e.Event)
                .ToListAsync();
        }  
        
        public async Task<List<Event>> FilterAdministeredEvents(int userId, string keyword)
        {
            return await FindByCondition(e => e.UserId == userId && e.Event.Title.Contains(keyword))
                .Select(e => e.Event)
                .ToListAsync();
        }
        
        public async Task<List<UserEventDto>> GetShortenedAdministeredEventsByUserId(int userId)
        {
            var events = await FindByCondition(e => e.UserId == userId)
                .Select(e => new UserEventDto
                {
                    Id = e.EventId,
                    Title = e.Event.Title,
                    Street = e.Event.Street,
                    StreetNumber = e.Event.StreetNumber,
                    City = e.Event.City,
                    AvatarPath = e.Event.Avatar,
                    ParticipantsAmount = e.Event.ParticipantsAmount,
                    Date = e.Event.Date
                })
                .ToListAsync();

            return events;
        }
    }
}