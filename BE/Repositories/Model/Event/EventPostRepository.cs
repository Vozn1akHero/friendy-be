using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Models;
using BE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class EventPostRepository: RepositoryBase<EventPost>, IEventPostRepository
    {
        public EventPostRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task CreateAsync(EventPost eventPost)
        {
            Create(eventPost);
            await SaveAsync();
        }

        public async Task<EventPost> GetByIdAsync(int id)
        {
            return await FindByCondition(e => e.Id == id)
                .Include(e => e.Event)
                .Include(e => e.Post)
                .Include(e => e.Post.PostLike)
                .Include(e => e.Post.Comment)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<EventPost>> GetRangeByIdAsync(int eventId, int startIndex, int length)
        {
            var posts = await FindByCondition(e => e.EventId == eventId && e.Id >= startIndex)
                .Take(length)
                .Include(e => e.Event)
                .Include(e => e.Post)
                .Include(e => e.Post.PostLike)
                .Include(e => e.Post.Comment)
                .OrderByDescending(e => e.Post.Date)
                .ToListAsync();
            return posts;
        }
    }
}