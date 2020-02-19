using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Helpers;
using BE.Helpers.CustomExceptions;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BE.Features.Event.Repositories
{
    public class EventRepository : RepositoryBase<Models.Event>, IEventRepository
    {
        public EventRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<TType> GetById<TType>(int id, Expression<Func<Models.Event,
            TType>> selector)
        {
            return await FindByCondition(e => e.Id == id)
                //.Include(e => e.EventParticipants)
                .Select(selector)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> IsUserCreatorById(int id, int userId)
        {
            return await Task.Run(() =>
                ExistsByCondition(e => e.Id == id && e.CreatorId == userId));
        }

        public async Task<object> GetWithSelectedFields(int id, string[] selectedFields)
        {
            var obj = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            if (obj == null) return null;
            return DynamicLinqStatement.ExtractSpecifiedFields(obj, selectedFields);
        }

        public void UpdateById<TType>(int id, TType entry)
        {
            var @event = FindByCondition(e => e.Id == id).SingleOrDefault();
            if (@event == null) throw new EntityWasntFoundException();
            foreach (var fromProp in typeof(Models.Event).GetProperties())
            {
                var toProp = typeof(TType).GetProperty(fromProp.Name);
                var toValue = toProp?.GetValue(entry, null);
                if (toValue != null) fromProp.SetValue(@event, toValue, null);
            }

            Update(@event);
        }

        public async Task<Models.Event> CreateAndReturn(Models.Event @event)
        {
            Create(@event);
            await SaveAsync();
            return @event;
        }

        public async Task<IEnumerable<Models.Event>> SearchByKeyword(string keyword)
        {
            return await FindByCondition(e => e.Title.Contains(keyword))
                .Include(e => e.EventParticipants)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Event>>
            FilterParticipatingByKeywordAndUserId(int userId, string keyword)
        {
            var events = await FindByCondition(e =>
                    e.EventParticipants.Any(d => d.ParticipantId == userId)
                    && e.Title.ToLower().Contains(keyword.ToLower()))
                .Include(e => e.EventParticipants)
                .ToListAsync();
            return events;
        }

        public async Task<IEnumerable<Models.Event>>
            FilterAdministeredByKeywordAndUserId(int userId, string keyword)
        {
            var events = await FindByCondition(e =>
                    e.EventAdmins.Any(d => d.UserId == userId)
                    && e.Title.ToLower().Contains(keyword.ToLower()))
                .Include(e => e.EventParticipants)
                .ToListAsync();
            return events;
        }

        public async Task<IEnumerable<Models.Event>> GetParticipatingByUserIdAsync(
            int userId)
        {
            var events =
                await FindByCondition(e =>
                        e.EventParticipants.Any(d => d.ParticipantId == userId))
                    .Include(e => e.EventParticipants)
                    .ToListAsync();
            return events;
        }

        public async Task<IEnumerable<Models.Event>> GetAdministeredByUserIdAsync(
            int userId)
        {
            var events =
                await FindByCondition(e => e.EventAdmins.Any(d => d.UserId == userId))
                    .Include(e => e.EventParticipants)
                    .ToListAsync();
            return events;
        }

        public async Task<IEnumerable<Models.Event>> GetClosestAsync(string city, int
            length, int issuerId)
        {
            var events =
                await FindByCondition(e => e.City == city && e.EventParticipants.All(d => d.ParticipantId != issuerId))
                    .Include(e => e.EventParticipants)
                    .Take(length)
                    .ToListAsync();
            return events;
        }
    }
}