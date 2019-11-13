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

        public async Task<List<Event>> GetEventsByUserId(int userId)
        {
            return await FindByCondition(e => e.UserId == userId)
                .Select(e => e.Event)
                .ToListAsync();
        }
        
        public async Task<List<UserEventDto>> GetShortenedEventsByUserId(int userId)
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