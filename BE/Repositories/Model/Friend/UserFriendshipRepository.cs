using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class UserFriendshipRepository : RepositoryBase<UserFriendship>,
        IUserFriendshipRepository
    {
        public UserFriendshipRepository(FriendyContext friendyContext) : base(
            friendyContext) {}

        public async Task AddNewAsync(int id, int userId)
        {
            var newFriend = new UserFriendship
            {
                FirstFriendId = userId,
                SecondFriendId = id
            };
            Create(newFriend);
            await SaveAsync();
        }

        public async Task<IEnumerable<UserFriendship>> GetRangeByUserIdAsync(int userId,
            int
                startIndex, int length)
        {
            var exampleFriendList = await FindByCondition(e => e.FirstFriendId == userId
                                                               || e.SecondFriendId ==
                                                               userId &&
                                                               e.Id >= startIndex)
                .Include(e => e.FirstFriend)
                .Include(e => e.SecondFriend)
                .ThenInclude(e => e.SessionNavigation)
                .Take(length)
                .ToListAsync();

            return exampleFriendList;
        }

        public async Task RemoveByIdentifiersAsync(int firstUserId, int secondUserId)
        {
            var friend = await FindByCondition(e =>
                    e.FirstFriendId == firstUserId && e.SecondFriendId == secondUserId
                    || e.FirstFriendId == secondUserId && e.SecondFriendId == firstUserId)
                .SingleOrDefaultAsync();
            Delete(friend);
            await SaveAsync();
        }

        public async Task<IEnumerable<UserFriendship>> GetExemplaryByUserIdAsync(
            int userId)
        {
            var friends = await FindByCondition(e =>
                    e.FirstFriendId == userId || e.SecondFriendId == userId)
                .Include(e => e.FirstFriend)
                .Include(e => e.SecondFriend)
                .Take(3)
                .ToListAsync();

            return friends;
        }

        public async Task<bool> CheckIfFriendsByUserIdsAsync(int firstUserId,
            int secondUserId)
        {
            return await Task.Run(() => ExistsByCondition(e =>
                e.FirstFriendId == firstUserId && e.SecondFriendId == secondUserId
                || e.SecondFriendId == firstUserId && e.FirstFriendId == secondUserId));
        }
    }
}