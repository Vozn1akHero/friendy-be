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

        public async Task<List<UserPost>> GetRangeByIdAsync(int userId, int startIndex, int length)
        {
           var posts = await FindByCondition(e => e.UserId == userId && e.Id > startIndex)
                .Include(e => e.Post.PostComment)
                .Include(e => e.Post.PostLike)
                .OrderByDescending(e => e.Post.Date)
                .Take(length)
                .ToListAsync();
           return posts;
        }
    }
}