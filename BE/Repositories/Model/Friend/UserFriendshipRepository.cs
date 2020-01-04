using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.FriendDtos;
using BE.Interfaces;
using BE.Interfaces.Repositories;
using BE.Models;
using BE.Services.Global.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class UserFriendshipRepository : RepositoryBase<UserFriendship>, IUserFriendshipRepository
    {
        private IRowSqlQueryService _rowSqlQueryService;
        
        public UserFriendshipRepository(FriendyContext friendyContext, 
            IRowSqlQueryService rowSqlQueryService) : base(friendyContext)
        {
            _rowSqlQueryService = rowSqlQueryService;
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
        
/*        public async Task<List<UserFriends>> FindAllByUserId(int userId)
        {
            var friends = await FindByCondition(e => e.FirstFriendId == userId || e.SecondFriendId == userId)
                .ToListAsync();
            
            return friends;
        }*/

        public async Task<List<FriendDto>> GetRangeByUserIdAsync(int userId, int startIndex, int length)
        {
            var exampleFriendList = await FindByCondition(e => e.FirstFriendId == userId || e.SecondFriendId == userId && e.Id >= startIndex)
                .Select(e => new FriendDto
                {
                    Id = e.FirstFriendId == userId ? e.SecondFriendId : e.FirstFriendId,
                    AvatarPath = e.FirstFriendId == userId ? e.SecondFriend.Avatar : e.FirstFriend.Avatar,
                    Name = e.FirstFriendId == userId ? e.SecondFriend.Name : e.FirstFriend.Name,
                    OnlineStatus = e.FirstFriendId == userId ? e.SecondFriend.SessionNavigation.ConnectionEnd != null 
                        : e.FirstFriend.SessionNavigation.ConnectionEnd != null,
                    Surname = e.FirstFriendId == userId ? e.SecondFriend.Surname : e.FirstFriend.Surname
                })
                .Take(length)
                .ToListAsync();
            
            return exampleFriendList;
        }

        public async Task<List<FriendDto>> FilterByKeywordAsync(int userId, string keyword)
        {
            string query =
                $"select u.id, u.avatar, u.name, u.surname, s.connection_end from user_friendship uf join [dbo].[user] u on " +
                $"u.id = ( select case when uf.first_friend_id <> {userId} then uf.first_friend_id when uf.second_friend_id <> {userId} " +
                $"then uf.second_friend_id end from user_friendship uf) join session s on u.session_id = s.id where u.name + u.surname like '%{keyword}%'";
            
            var filteredFriends = _rowSqlQueryService.Execute(query, e => new FriendDto{
                Id = (int)e[0],
                AvatarPath = (string)e[1],
                Name = (string)e[2],
                Surname = (string)e[3],
                OnlineStatus = (DateTime)e[4] == null
            });
            
            return filteredFriends;
        }

        public async Task RemoveByIdentifiersAsync(int firstUserId, int secondUserId)
        {
            var friend = await FindByCondition(e => e.FirstFriendId == firstUserId && e.SecondFriendId == secondUserId 
                                                    || e.FirstFriendId == secondUserId && e.SecondFriendId == firstUserId).SingleOrDefaultAsync();
            Delete(friend);
            await SaveAsync();
        }

        public async Task<List<ExemplaryFriendDto>> GetExemplaryByUserIdAsync(int userId)
        {
            var friends = await FindByCondition(e => e.FirstFriendId == userId || e.SecondFriendId == userId)
                .Select(e => new ExemplaryFriendDto
                {
                    Id = e.FirstFriendId == userId ? e.SecondFriendId : e.FirstFriendId,
                    AvatarPath = e.FirstFriendId == userId ? e.SecondFriend.Avatar : e.FirstFriend.Avatar
                })
                .Take(3)
                .ToListAsync();

            return friends;
        }

        public async Task<bool> CheckIfFriendsByUserIdsAsync(int firstUserId, int secondUserId)
        {
            return await Task.Run(() => ExistsByCondition(e =>
                e.FirstFriendId == firstUserId && e.SecondFriendId == secondUserId
                || e.SecondFriendId == firstUserId && e.FirstFriendId == secondUserId));
        }
    }
}