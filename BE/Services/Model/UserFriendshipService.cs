using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.FriendDtos;
using BE.Interfaces;
using BE.Models;
using BE.Services.Global;

namespace BE.Services.Model
{
    public interface IUserFriendshipService
    {
        Task<IEnumerable<FriendDto>> GetRangeByUserIdAsync(int userId,
            int startIndex, int length);
        Task<IEnumerable<FriendDto>> GetLastRangeByIdWithPaginationAsync(int id, int page);
        Task<IEnumerable<FriendDto>> FilterByKeywordAsync(int userId, string keyword);
        Task<IEnumerable<ExemplaryFriendDto>> GetExemplaryByUserIdAsync(int userId);
    }
    
    public class UserFriendshipService : IUserFriendshipService
    {
        private IRepositoryWrapper _repository;
        private IRawSqlQuery _rawSqlQuery;

        public UserFriendshipService(IRepositoryWrapper repository,
            IRawSqlQuery rawSqlQuery)
        {
            _repository = repository;
            _rawSqlQuery = rawSqlQuery;
        }
        
        

        public async Task<IEnumerable<FriendDto>> GetRangeByUserIdAsync(int userId,
            int startIndex, int length)
        {
            var fr = await _repository.UserFriendship.GetRangeByUserIdAsync(userId,
                startIndex, length);
            var res = fr.Select(e => new FriendDto
            {
                Id = e.FirstFriendId == userId ? e.SecondFriendId : e.FirstFriendId,
                AvatarPath = e.FirstFriendId == userId
                    ? e.SecondFriend.Avatar
                    : e.FirstFriend.Avatar,
                Name = e.FirstFriendId == userId
                    ? e.SecondFriend.Name
                    : e.FirstFriend.Name,
                OnlineStatus = e.FirstFriendId == userId
                    ? e.SecondFriend.SessionNavigation.ConnectionEnd != null
                    : e.FirstFriend.SessionNavigation.ConnectionEnd != null,
                Surname = e.FirstFriendId == userId
                    ? e.SecondFriend.Surname
                    : e.FirstFriend.Surname
            });
            return res;
        }

        public async Task<IEnumerable<FriendDto>> GetLastRangeByIdWithPaginationAsync(int id, int page)
        {
            var fr = await _repository.UserFriendship.GetLastRangeByIdWithPaginationAsync(id, page);
            var res = fr.Select(e => new FriendDto
            {
                Id = e.FirstFriendId == id ? e.SecondFriendId : e.FirstFriendId,
                AvatarPath = e.FirstFriendId == id
                    ? e.SecondFriend.Avatar
                    : e.FirstFriend.Avatar,
                Name = e.FirstFriendId == id
                    ? e.SecondFriend.Name
                    : e.FirstFriend.Name,
                OnlineStatus = e.FirstFriendId == id
                    ? e.SecondFriend.SessionNavigation.ConnectionEnd != null
                    : e.FirstFriend.SessionNavigation.ConnectionEnd != null,
                Surname = e.FirstFriendId == id
                    ? e.SecondFriend.Surname
                    : e.FirstFriend.Surname
            });
            return res;
        }

        public async Task<IEnumerable<FriendDto>> FilterByKeywordAsync(int userId, string keyword)
        {
            string query =
                $"select u.id, u.avatar, u.name, u.surname, s.connection_end from user_friendship uf join [dbo].[user] u on " +
                $"u.id = ( select case when uf.first_friend_id <> {userId} then uf.first_friend_id when uf.second_friend_id <> {userId} " +
                $"then uf.second_friend_id end from user_friendship uf) join session s on u.session_id = s.id where u.name + u.surname like '%{keyword}%'";
            
            var filteredFriends = _rawSqlQuery.Execute(query, e => new FriendDto{
                Id = (int)e[0],
                AvatarPath = (string)e[1],
                Name = (string)e[2],
                Surname = (string)e[3],
                OnlineStatus = (DateTime)e[4] == null
            });
            
            return filteredFriends;
        }

        public async Task<IEnumerable<ExemplaryFriendDto>> GetExemplaryByUserIdAsync(int userId)
        {
            var fr = await _repository.UserFriendship.GetExemplaryByUserIdAsync(userId);
            var res = fr.Select(e => new ExemplaryFriendDto
            {
                Id = e.FirstFriendId == userId ? e.SecondFriendId : e.FirstFriendId,
                AvatarPath = e.FirstFriendId == userId
                    ? e.SecondFriend.Avatar
                    : e.FirstFriend.Avatar
            });
            return res;
        }
    }
}