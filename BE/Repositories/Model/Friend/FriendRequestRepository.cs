using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class FriendRequestRepository : RepositoryBase<FriendRequest>, IFriendRequestRepository
    {
        public FriendRequestRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<FriendRequest> FindByUserIds(int authorId, int receiverId)
        {
            return await FindByCondition(e => e.AuthorId == authorId && e.ReceiverId == receiverId)
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
            return Task.Run(() => { return ExistsByCondition(e => e.AuthorId == firstId && e.ReceiverId == secondId
                                                           || e.ReceiverId == firstId && e.AuthorId == secondId); });
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
    }
}