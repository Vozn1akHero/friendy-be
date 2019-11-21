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
        
        public async Task<IEnumerable<EventPostOnWallDto>> GetRangeByIdAsync(int eventId, int startIndex, int length, int userId)
        {
            var posts = await FindByCondition(e => e.EventId == eventId && e.Id >= startIndex)
                .Select(e => new EventPostOnWallDto
                {
                    Id = e.Id,
                    CommentsCount = e.Post.Comment.Count,
                    LikesCount = e.Post.PostLike.Count,
                    Content = e.Post.Content,
                    Date = e.Post.Date,
                    ImagePath = e.Post.ImagePath,
                    PostId = e.PostId,
                    IsPostLikedByUser = e.Post.PostLike
                        .ToList()
                        .Exists(like => like.PostId == e.PostId && like.UserId == userId),
                    EventId = e.EventId
                })
                .Take(length)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
            return posts;
        }
    }
}