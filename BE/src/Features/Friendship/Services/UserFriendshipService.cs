using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.FriendDtos;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Friendship.Services
{
    public interface IUserFriendshipService
    {
        Task<IEnumerable<FriendDto>> GetRangeByUserIdAsync(int userId,
            int startIndex, int length);

        Task<IEnumerable<FriendDto>>
            GetLastRangeByIdWithPaginationAsync(int id, int page);

        Task<IEnumerable<FriendDto>> FilterByKeywordAsync(int userId, string keyword);
        Task<IEnumerable<ExemplaryFriendDto>> GetExemplaryByUserIdAsync(int userId);
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
            int id, int page)
        {
            var fr =
                await _repository.UserFriendship.GetLastRangeByIdWithPaginationAsync(id,
                    page);
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

        public async Task<IEnumerable<ExemplaryFriendDto>> GetExemplaryByUserIdAsync(
            int userId)
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