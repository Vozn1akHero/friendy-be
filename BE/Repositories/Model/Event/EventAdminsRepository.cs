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
        
        public async Task<List<Models.Event>> GetUserAdministeredEvents(int userId)
        {
            return await FindByCondition(e => e.UserId == userId)
                .Select(e => e.Event)
                .ToListAsync();
        }  
        
        public async Task<List<Models.Event>> FilterAdministeredEvents(int userId, string keyword)
        {
            return await FindByCondition(e => e.UserId == userId && e.Event.Title.Contains(keyword))
                .Select(e => e.Event)
                .ToListAsync();
        }
        
        public async Task<List<EventDto>> GetShortenedAdministeredEventsByUserId(int userId)
        {
            var events = await FindByCondition(e => e.UserId == userId)
                .Select(e => new EventDto
                {
                    Id = e.EventId,
                    Title = e.Event.Title,
                    Street = e.Event.Street,
                    StreetNumber = e.Event.StreetNumber,
                    City = e.Event.City,
                    AvatarPath = e.Event.Avatar,
                    ParticipantsAmount = e.Event.ParticipantsAmount,
                    CurrentParticipantsAmount = e.Event.EventParticipants.Count,
                    Date = e.Event.Date
                })
                .ToListAsync();

            return events;
        }

        public Task<bool> IsUserAdminById(int eventId, int userId)
        {
            return Task.Run(() => ExistsByCondition(e => e.EventId == eventId && e.UserId == userId));
        }
    }
}