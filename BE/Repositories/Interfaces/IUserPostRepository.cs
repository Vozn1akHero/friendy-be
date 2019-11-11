using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories
{
    public interface IUserPostRepository : IRepositoryBase<UserPost>
    {
        Task CreateAsync(UserPost post);
        Task RemoveByIdAsync(int id);
        Task<UserPost> GetByIdAsync(int id);
        Task<List<UserPost>> GetRangeByIdAsync(int userId, int startIndex, int length);
    }
}