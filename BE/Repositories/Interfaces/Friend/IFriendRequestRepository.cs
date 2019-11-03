using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IFriendRequestRepository
    {
        Task Add(int authorId, int receiverId);
        Task<List<FriendRequest>> GetReceivedByUserId(int userId);
        Task<List<FriendRequest>> GetSentByUserId(int userId);
    }
}