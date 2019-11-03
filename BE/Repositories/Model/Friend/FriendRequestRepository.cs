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