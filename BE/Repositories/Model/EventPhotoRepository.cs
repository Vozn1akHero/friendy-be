using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.PhotoDtos;
using BE.Models;
using BE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class EventPhotoRepository: RepositoryBase<EventImage>, IEventPhotoRepository
    {
        public EventPhotoRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<IEnumerable<EventImage>> GetRange(int eventId, int 
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

        public async Task<int> GetPicturesAmountByEventId(int eventId)
        {
            return await Task.Run(() => FindByCondition(e => e.EventId == eventId).Count());
        }
    }
}