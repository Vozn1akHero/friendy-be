using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.FriendDtos;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class UserFriendsRepository : RepositoryBase<UserFriends>, IUserFriendsRepository
    {
        public UserFriendsRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task AddNew(int id, int userId)
        {
            var newFriend = new UserFriends
            {
                UserId = userId,
                FriendId = id
            };
            Create(newFriend);
            await SaveAsync();
        }
        
        public async Task<List<UserFriends>> FindAllByUserId(int userId)
        {
            var friends = await FindByCondition(e => e.UserId == userId)
                .Include(e => e.Friend)
                .ToListAsync();
            
            return friends;
        }
        
        public async Task<List<UserFriends>> GetIndexedByUserId(int userId, int startIndex, int lastIndex)
        {
            var exampleFriendList = await FindByCondition(e => e.UserId == userId && e.Id >= startIndex && e.Id <= lastIndex)
                .Include(e => e.Friend.FriendNavigation)
                .ToListAsync();
            
            return exampleFriendList;
        }
        
        public async Task<List<ShortenedFriendDto>> GetIndexedShortenedByUserId(int userId, int startIndex, int lastIndex)
        {
            var exampleFriendList = await FindByCondition(e => e.UserId == userId && e.Id >= startIndex && e.Id <= lastIndex)
                .Include(e => e.Friend.FriendNavigation)
                .Select(e => new ShortenedFriendDto
                {
                    Id = e.FriendId,
                    AvatarPath = e.Friend.FriendNavigation.Avatar,
                    Name = e.Friend.FriendNavigation.Name,
                    Surname = e.Friend.FriendNavigation.Surname
                })
                .ToListAsync();
            
            return exampleFriendList;
        }

        public async Task<List<UserFriends>> FilterByKeyword(int userId, string keyword)
        {
            var filteredFriends = await FindByCondition(e => e.UserId == userId)
                .Include(e => e.Friend.FriendNavigation)
                .Where(e =>
                    e.Friend.FriendNavigation.Name.Contains(keyword)
                    || e.Friend.FriendNavigation.Surname.Contains(keyword))
                .ToListAsync();
            
            return filteredFriends;
        }

        public async Task RemoveById(int id)
        {
            var friend = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            Delete(friend);
            await SaveAsync();
        }

        public async Task<List<UserFriends>> GetExemplaryByUserId(int userId)
        {
            var friends = await FindByCondition(e => e.UserId == userId)
                .Include(e => e.Friend.FriendNavigation)
                .Take(3)
                .ToListAsync();

            return friends;
        }

        public bool CheckIfFriendsByUserIds(int firstUserId, int secondUserId)
        {
            return ExistsByCondition(e => (e.UserId == firstUserId && e.Friend.FriendId == secondUserId)
                                          || (e.Friend.FriendId == firstUserId &&
                                              e.UserId == secondUserId));
        }
    }
}