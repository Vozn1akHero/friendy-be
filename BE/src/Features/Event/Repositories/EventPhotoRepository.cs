using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Event.Repositories
{
    public class EventPhotoRepository : RepositoryBase<EventImage>, IEventPhotoRepository
    {
        public EventPhotoRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<IEnumerable<TType>> SelectWithPaginationAsync<TType>(
            int eventId,
            int
                page, Expression<Func<EventImage, TType>> selector)
        {
            var length = 25;
            return await FindByCondition(e => e.EventId == eventId)
                .Skip((page - 1) * length)
                .Take(length)
                .Select(selector)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventImage>> GetRangeAsync(int eventId, int
            startIndex, int length)
        {
            return await FindByCondition(e => e.EventId == eventId && e.Id >= startIndex)
                .Include(e => e.Image)
                .Take(length)
                .ToListAsync();
        }

        public async Task Add(int eventId, int photoId)
        {
            Create(new EventImage
            {
                EventId = eventId,
                ImageId = photoId
            });
            await SaveAsync();
        }
    }
}