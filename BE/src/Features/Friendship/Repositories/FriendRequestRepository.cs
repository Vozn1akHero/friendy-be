using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.FriendRequestDtos;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Friendship.Repositories
{
    public class FriendRequestRepository : RepositoryBase<FriendRequest>,
        IFriendRequestRepository
    {
        public FriendRequestRepository(FriendyContext friendyContext) : base(
            friendyContext)
        {
        }

        public async Task<FriendRequest> FindByUserIds(int authorId, int receiverId)
        {
            return await FindByCondition(e =>
                    e.AuthorId == authorId && e.ReceiverId == receiverId)
                .SingleOrDefaultAsync();
        }

        public async Task Add(int authorId, int receiverId)
        {
            var newFriendRequest = new FriendRequest
            {
                AuthorId = authorId,
                ReceiverId = receiverId
            };
            Create(newFriendRequest);
            await SaveAsync();
        }

        public async Task DeleteByEntity(FriendRequest friendRequest)
        {
            Delete(friendRequest);
            await SaveAsync();
        }

        public Task<bool> GetStatusByUserIds(int firstId, int secondId)
        {
            return Task.Run(() =>
            {
                return ExistsByCondition(e =>
                    e.AuthorId == firstId && e.ReceiverId == secondId
                    || e.ReceiverId == firstId && e.AuthorId == secondId);
            });
        }

        public async Task<List<FriendRequest>> GetReceivedByUserId(int userId)
        {
            return await FindByCondition(e => e.ReceiverId == userId)
                .ToListAsync();
        }

        public async Task<List<FriendRequest>> GetSentByUserId(int userId)
        {
            return await FindByCondition(e => e.AuthorId == userId)
                .ToListAsync();
        }

        public async Task<List<ReceivedFriendRequestDto>> GetReceivedByUserIdWithDto(
            int userId)
        {
            return await FindByCondition(e => e.ReceiverId == userId)
                .Select(e => new ReceivedFriendRequestDto
                {
                    RequestId = e.Id,
                    Name = e.Author.Name,
                    Surname = e.Author.Surname,
                    AvatarPath = e.Receiver.Avatar,
                    AuthorId = e.AuthorId
                })
                .ToListAsync();
        }

        public async Task<List<SentFriendRequestDto>> GetSentByUserIdWithDto(int userId)
        {
            return await FindByCondition(e => e.AuthorId == userId)
                .Select(e => new SentFriendRequestDto
                {
                    RequestId = e.Id,
                    Name = e.Receiver.Name,
                    Surname = e.Receiver.Surname,
                    AvatarPath = e.Receiver.Avatar,
                    ReceiverId = e.Receiver.Id
                })
                .ToListAsync();
        }
    }
}