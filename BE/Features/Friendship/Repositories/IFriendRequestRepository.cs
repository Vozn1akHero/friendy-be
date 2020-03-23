using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.Friendship.Dtos;
using BE.Models;

namespace BE.Features.Friendship.Repositories
{
    public interface IFriendRequestRepository
    {
        Task<FriendRequest> FindByUserIds(int authorId, int receiverId);
        Task Add(int authorId, int receiverId);
        Task<IEnumerable<TType>> GetReceivedByUserIdAsync<TType>(int userId,
            Expression<Func<FriendRequest, TType>> selector);
        Task<IEnumerable<TType>> GetSentByUserIdAsync<TType>(int userId,
            Expression<Func<FriendRequest, TType>> selector);
        Task DeleteByEntity(FriendRequest friendRequest);
        bool GetStatusByUserIds(int firstId, int secondId);
    }
}