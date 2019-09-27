using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IUserFriendsRepository : IRepositoryBase<UserFriends>
    {
        Task<List<UserFriends>> FindAllByUserId(int userId);
        Task<List<UserFriends>> GetByUserId(int userId, int startIndex, int lastIndex);
        Task<List<UserFriends>> FilterByKeyword(int userId, string keyword);
        Task RemoveById(int id);
        Task<List<UserFriends>> GetExemplaryByUserId(int userId);
    }
}