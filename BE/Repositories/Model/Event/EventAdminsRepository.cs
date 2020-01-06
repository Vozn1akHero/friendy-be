using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos;
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

        public async Task CreateAndReturn(int eventId, int userId)
        {
            Create(new EventAdmins()
            {
                EventId = eventId,
                UserId = userId
            });
            await SaveAsync();
        }

        public async Task<List<EventAdminDto>> GetByEventIdAsync(int eventId)
        {
            return await FindByCondition(e => e.EventId == eventId)
                .Select(e => new EventAdminDto
                    {
                        Id = e.UserId,
                        IsEventCreator = e.Event.CreatorId == e.UserId,
                        Name = e.User.Name,
                        Surname = e.User.Surname,
                        City = e.User.City,
                        GenderId = e.User.GenderId,
                        Avatar = e.User.Avatar
                    }).ToListAsync();
        }

        public async Task DeleteByEventIdAndAdminId(int eventId, int adminId)
        {
            var foundEntity = await FindByCondition(e => e.EventId == eventId && e.UserId == adminId)
                .SingleOrDefaultAsync();
            if (foundEntity != null)
            {
                Delete(foundEntity);
                await SaveAsync();
            }
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