using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.FriendDtos;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IUserFriendshipRepository : IRepositoryBase<UserFriendship>
    {
        //Task<List<UserFriendship>> FindAllByUserId(int userId);
        //Task<List<UserFriends>> GetIndexedByUserId(int userId, int startIndex, int lastIndex);
        Task<IEnumerable<UserFriendship>> GetRangeByUserIdAsync(int userId, int
            startIndex, int length);
        //Task<IEnumerable<FriendDto>> FilterByKeywordAsync(int userId, string keyword);
        Task RemoveByIdentifiersAsync(int firstUserId, int secondUserId);
        Task<IEnumerable<UserFriendship>> GetExemplaryByUserIdAsync(int userId);
        Task AddNewAsync(int id, int userId);
        Task<bool> CheckIfFriendsByUserIdsAsync(int firstUserId, int secondUserId);
    }
}