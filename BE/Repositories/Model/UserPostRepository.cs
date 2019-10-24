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
            : base(friendyContext)
        { }

        public async Task CreateUserPost(UserPost post)
        {
            Create(post);
            await SaveAsync();
        }

        public async Task RemovePostById(int id)
        {
            var post = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            Delete(post);
            await SaveAsync();
        }

        public async Task<List<UserPost>> GetById(int id)
        {
           var posts = await FindByCondition(e => e.UserId == id)
                .Include("UserPostLikes")
                .Include("UserPostComments")
                .OrderByDescending(e => e.Date)
                .ToListAsync();
           return posts;
        }
    }
}