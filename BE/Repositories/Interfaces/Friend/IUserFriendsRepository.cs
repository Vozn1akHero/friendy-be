using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.FriendDtos;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IUserFriendsRepository : IRepositoryBase<UserFriends>
    {
        Task<List<UserFriends>> FindAllByUserId(int userId);
        //Task<List<UserFriends>> GetIndexedByUserId(int userId, int startIndex, int lastIndex);
        Task<List<FriendDto>> GetIndexedByUserId(int userId, int startIndex, int lastIndex);
        Task<List<UserFriends>> FilterByKeyword(int userId, string keyword);
        Task RemoveById(int id);
        Task<List<ExemplaryFriendDto>> GetExemplaryByUserId(int userId);
        Task AddNew(int id, int userId);
        bool CheckIfFriendsByUserIds(int firstUserId, int secondUserId);
    }
}