using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Helpers;
using BE.Interfaces;
using BE.Interfaces.Repositories;
using BE.Models;
using BE.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BE.Repositories
{
    public class EventRepository : RepositoryBase<Models.Event>, IEventRepository
    {
        public EventRepository(FriendyContext friendyContext) : base(friendyContext) { }

        public async Task<List<Models.Event>> GetExampleEventsByUserId(int userId)
        {
/*
            var events = await FindByCondition(e => e.EventParticipants.Part)
                .Include(e => e.UserEvents)
                .ToListAsync();
            
            */

            return null;
        }

        public async Task<Models.Event> GetById(int id)
        {
            return await FindByCondition(e => e.Id == id)
                .Include(e => e.EventParticipants)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<EventDto>> SearchByKeyword(string keyword)
        {
            return await FindByCondition(e => e.Title.Contains(keyword))
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    AvatarPath = e.Avatar,
                    City = e.City,
                    Date = e.Date,
                    ParticipantsAmount = e.ParticipantsAmount,
                    CurrentParticipantsAmount = e.EventParticipants.Count,
                    Title = e.Title,
                    Street = e.Street,
                    StreetNumber = e.StreetNumber
                })
                .ToListAsync();
        }

        public async Task<bool> IsUserCreatorById(int id, int userId)
        {
            return await Task.Run(() => ExistsByCondition(e => e.Id == id && e.CreatorId == userId));
        }

        public async Task<object> GetWithSelectedFields(int id, string[] selectedFields)
        {
            var obj = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            return DynamicLinqStatement.ExtractSpecifiedFields<Models.Event>(obj, selectedFields);
        }

        public async Task<string> GetAvatarPathByEventIdAsync(int id)
        {
            return await FindByCondition(e => e.Id == id).Select(e => e.Avatar).SingleOrDefaultAsync();
        }
        
        public async Task UpdateAvatarAsync(string path, int id)
        {
            var obj = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            obj.Avatar = path;
            await SaveAsync();
        }

        public async Task UpdateBackgroundAsync(string path, int id)
        {
            var obj = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            obj.Background = path;
            await SaveAsync();
        }

        public async Task<IEnumerable<Models.Event>> FilterParticipatingByKeywordAndUserId(int userId, string keyword)
        {
            var events = await FindByCondition(e =>
                e.EventParticipants.Any(d => d.ParticipantId == userId) && e.Title.ToLower().Contains(keyword.ToLower()))
                .ToListAsync();
            return events;
        }
        
        
        public async Task<IEnumerable<Models.Event>> FilterAdministeredByKeywordAndUserId(int userId, string keyword)
        {
            var events = await FindByCondition(e =>
                    e.EventAdmins.Any(d => d.UserId == userId) && e.Title.ToLower().Contains(keyword.ToLower()))
                .ToListAsync();
            return events;
        }
    }
}