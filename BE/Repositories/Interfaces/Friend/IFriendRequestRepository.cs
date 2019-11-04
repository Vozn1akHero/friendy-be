using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IFriendRequestRepository
    {
        Task<FriendRequest> FindByUserIds(int authorId, int receiverId);
        Task Add(int authorId, int receiverId);
        Task<List<FriendRequest>> GetReceivedByUserId(int userId);
        Task<List<FriendRequest>> GetSentByUserId(int userId);
        Task DeleteByEntity(FriendRequest friendRequest);
        Task<bool> GetStatusByUserIds(int firstId, int secondId);
    }
}