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
        
        public async Task<IEnumerable<UserPost>> GetRangeByIdAsync(int userId, int startIndex, int length)
        {
            var posts = await FindByCondition(e => e.UserId == userId && e.Id >= startIndex)
                .Include(e => e.Post)
                .Include(e => e.Post.PostLike)
                .Include(e => e.Post.Comment)
                .Include(e => e.User)
                .Take(length)
                .OrderByDescending(e => e.Post.Date)
                .ToListAsync();
            return posts;
        }
        
        public async Task CreateAsync(UserPost post)
        {
            Create(post);
            await SaveAsync();
        }

        public async Task<UserPost> CreateAndReturnAsync(UserPost post)
        {
            Create(post);
            await SaveAsync();
            return post;
        }

        public async Task RemoveByIdAsync(int id)
        {
            var post = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            Delete(post);
            await SaveAsync();
        }

        public async Task<UserPost> GetByIdAsync(int id)
        {
            return await FindByCondition(e => e.Id == id)
                .Include(e => e.Post)
                .Include(e => e.Post.PostLike)
                .Include(e => e.Post.Comment)
                .Include(e => e.User)
                .SingleOrDefaultAsync();
        }

        public async Task<UserPost> GetByPostIdAsync(int postId)
        {
            return await FindByCondition(e => e.PostId == postId)
                .Include(e => e.Post)
                .Include(e => e.Post.PostLike)
                .Include(e => e.Post.Comment)
                .Include(e => e.User)
                .SingleOrDefaultAsync();
        }
    }
}