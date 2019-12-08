using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;


namespace BE.Repositories
{
    public class UserPostRepository : RepositoryBase<UserPost>, IUserPostRepository
    {
        public UserPostRepository(FriendyContext friendyContext)
            : base(friendyContext) { }

        public async Task CreateAsync(UserPost post)
        {
            Create(post);
            await SaveAsync();
        }

        public async Task<UserPostDto> CreateAndReturnAsync(UserPost post)
        {
            Create(post);
            await SaveAsync();
            var newPost = await FindByCondition(e => e.Id == post.Id)
                .Select(e => new UserPostDto
                {
                    Id = e.Id,
                    CommentsCount = 0,
                    LikesCount = 0,
                    Content = e.Post.Content,
                    Date = e.Post.Date,
                    ImagePath = e.Post.ImagePath,
                    PostId = e.PostId,
                    IsPostLikedByUser = false,
                    Avatar = e.User.Avatar,
                    UserId = e.UserId
                })
                .SingleOrDefaultAsync();
            return newPost;
        }

        public async Task RemoveByIdAsync(int id)
        {
            var post = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            Delete(post);
            await SaveAsync();
        }

        public async Task<UserPost> GetByIdAsync(int id)
        {
            return await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<UserPostDto>> GetRangeByIdAsync(int userId, int startIndex, int length)
        {
           var posts = await FindByCondition(e => e.UserId == userId && e.Id >= startIndex)
               .Select(e => new UserPostDto
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
                   Avatar = e.User.Avatar,
                   UserId = e.UserId
               })
               .Take(length)
               .OrderByDescending(e => e.Date)
               .ToListAsync();
           return posts;
        }
    }
}