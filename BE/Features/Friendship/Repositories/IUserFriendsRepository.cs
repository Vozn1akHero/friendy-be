using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Friendship.Repositories
{
    public interface IUserFriendshipRepository : IRepositoryBase<UserFriendship>
    {
        Task<IEnumerable<UserFriendship>> GetRangeByUserIdAsync(int userId, int
            startIndex, int length);

        Task<IEnumerable<UserFriendship>> GetLastRangeByIdWithPaginationAsync(int id,
            int page, int length);
        Task RemoveByIdentifiersAsync(int firstUserId, int secondUserId);
        Task AddNewAsync(int id, int userId);
        Task<bool> CheckIfFriendsByUserIdsAsync(int firstUserId, int secondUserId);

        Task<IEnumerable<UserFriendship>> FindByName(int id,
            string keyword);
    }
}