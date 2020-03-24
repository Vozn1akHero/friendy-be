using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Features.Friendship.Dtos;
using BE.Features.Friendship.Helpers;
using BE.Models;
using BE.Repositories;
using BE.Shared;

namespace BE.Features.Friendship.Services
{
    public interface IUserFriendshipService
    {
        Task<IEnumerable<FriendDto>> GetRangeByUserIdAsync(int userId,
            int startIndex, int length);
        Task<IEnumerable<FriendDto>>
            GetLastRangeByIdWithPaginationAsync(int id, int page, int length);
        Task<IEnumerable<FriendDto>> FilterByKeywordAsync(int userId, string keyword);
        Task<FriendshipStatus> GetFriendshipStatus(int id, int issuerId);
        Task ConfirmRequestAsync(int userId, int issuerId);
    }

    public class UserFriendshipService : IUserFriendshipService
    {
        private readonly IRepositoryWrapper _repository;

        public UserFriendshipService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FriendDto>> GetRangeByUserIdAsync(int userId,
            int startIndex, int length)
        {
            var fr = await _repository.UserFriendship.GetRangeByUserIdAsync(userId,
                startIndex, length);
            var res = ConvertFriendshipEnumerable(userId, fr);
            return res;
        }

        public async Task<IEnumerable<FriendDto>> GetLastRangeByIdWithPaginationAsync(
            int id, int page, int length)
        {
            var fr =
                await _repository.UserFriendship.GetLastRangeByIdWithPaginationAsync(id,
                    page, length);
            var res = ConvertFriendshipEnumerable(id, fr);
            return res;
        }

        public async Task<IEnumerable<FriendDto>> FilterByKeywordAsync(int userId,
            string keyword)
        {
            var filteredFriends = await _repository.UserFriendship.FindByName
                (userId, keyword);
            var res = ConvertFriendshipEnumerable(userId, filteredFriends);
            return res;
        }

        public async Task<FriendshipStatus> GetFriendshipStatus(int id, int issuerId)
        {
            bool friendshipStatus =
                await _repository.UserFriendship.CheckIfFriendsByUserIdsAsync(id, issuerId);
            if (friendshipStatus)
            {
                return FriendshipStatus.FRIEND;
            }
            bool requestStatus = _repository.FriendRequest.GetStatusByUserIds(id, issuerId);
            if (requestStatus) return FriendshipStatus.REQUEST_SENT;
            return FriendshipStatus.NOT_FRIEND;
        }

        public async Task ConfirmRequestAsync(int userId, int issuerId)
        {
            bool status = _repository.FriendRequest.GetStatusByUserIds(userId, issuerId);
            if (!status) throw new FriendshipRequestNotFound();
            await _repository.UserFriendship.AddNewAsync(userId, issuerId);
            await _repository.Chat.Add(userId, issuerId);
            await _repository.SaveAsync();
        }

        private IEnumerable<FriendDto> ConvertFriendshipEnumerable
            (int userId, IEnumerable<UserFriendship> friendships)
        {
            var res = friendships.Select(e => new FriendDto
            {
                Id = e.FirstFriendId == userId ? e.SecondFriendId : e.FirstFriendId,
                AvatarPath = e.FirstFriendId == userId
                    ? e.SecondFriend.Avatar
                    : e.FirstFriend.Avatar,
                Name = e.FirstFriendId == userId
                    ? e.SecondFriend.Name
                    : e.FirstFriend.Name,
                OnlineStatus = e.FirstFriendId == userId
                    ? e.SecondFriend.Session.ConnectionEnd == null
                    : e.FirstFriend.Session.ConnectionEnd == null,
                Surname = e.FirstFriendId == userId
                    ? e.SecondFriend.Surname
                    : e.FirstFriend.Surname
            }).ToList();
            return res;
        }
    }
}