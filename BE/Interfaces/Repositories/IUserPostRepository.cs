using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories
{
    public interface IUserPostRepository : IRepositoryBase<UserPost>
    {
        Task CreateUserPost(UserPost post);
        Task RemovePostById(int id);
        Task<List<UserPost>> GetLoggedInUserPostsById(int id);
    }
}