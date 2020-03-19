using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Friendship.Repositories
{
    public class UserFriendshipRepository : RepositoryBase<UserFriendship>,
        IUserFriendshipRepository
    {
        public UserFriendshipRepository(FriendyContext friendyContext) : base(
            friendyContext)
        {
        }

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
            int startIndex, int length)
        {
            var exampleFriendList = await FindByCondition(e => e.FirstFriendId == userId
                                                               || e.SecondFriendId ==
                                                               userId &&
                                                               e.Id >= startIndex)
                .Include(e => e.FirstFriend)
                .ThenInclude(e => e.Session)
                .Include(e => e.SecondFriend)
                .ThenInclude(e => e.Session)
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
        }

        public async Task<IEnumerable<UserFriendship>>
            GetLastRangeByIdWithPaginationAsync(int id, int page, int length)
        {
            return await FindByCondition(
                    e => e.FirstFriendId == id || e.SecondFriendId == id)
                .Include(e => e.FirstFriend)
                .ThenInclude(e => e.Session)
                .Include(e => e.SecondFriend)
                .ThenInclude(e => e.Session)
                .OrderByDescending(e => e.Id)
                .Skip((page - 1) * length)
                .Take(length)
                .ToListAsync();
        }

        public async Task<bool> CheckIfFriendsByUserIdsAsync(int firstUserId,
            int secondUserId)
        {
            return await Task.Run(() => ExistsByCondition(e =>
                e.FirstFriendId == firstUserId && e.SecondFriendId == secondUserId
                || e.SecondFriendId == firstUserId && e.FirstFriendId == secondUserId));
        }

        public async Task<IEnumerable<UserFriendship>> FindByName(int id,
            string keyword)
        {
            var res = await FindByCondition(e => e.FirstFriendId == id && (e.SecondFriend
                                                                               .Name +
                                                                           " " +
                                                                           e.SecondFriend
                                                                               .Surname)
                                                 .StartsWith(keyword) || e
                                                     .SecondFriendId == id &&
                                                 (e.FirstFriend
                                                      .Name + " " +
                                                  e.FirstFriend.Surname)
                                                 .StartsWith(keyword))
                .Include(e => e.FirstFriend)
                .ThenInclude(e => e.Session)
                .Include(e => e.SecondFriend)
                .ThenInclude(e => e.Session)
                .ToListAsync();
            return res;
        }
    }
}