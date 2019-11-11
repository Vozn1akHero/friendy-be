using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task CreateAsync(Post post);
        Task RemoveByIdAsync(int id);
        Task<List<UserPost>> GetByIdAsync(int id);
    }
}