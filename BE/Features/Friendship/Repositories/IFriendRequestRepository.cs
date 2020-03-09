using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Features.Friendship.Dtos;
using BE.Models;

namespace BE.Features.Friendship.Repositories
{
    public interface IFriendRequestRepository
    {
        Task<FriendRequest> FindByUserIds(int authorId, int receiverId);
        Task Add(int authorId, int receiverId);
        Task<List<FriendRequest>> GetReceivedByUserId(int userId);
        Task<List<FriendRequest>> GetSentByUserId(int userId);
        Task DeleteByEntity(FriendRequest friendRequest);
        bool GetStatusByUserIds(int firstId, int secondId);
        Task<List<SentFriendRequestDto>> GetSentByUserIdWithDto(int userId);
        Task<List<ReceivedFriendRequestDto>> GetReceivedByUserIdWithDto(int userId);
    }
}