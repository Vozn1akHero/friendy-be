using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.Friendship.Dtos;
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

        public async Task<IEnumerable<TType>> FilterSentByKeywordAndUserIdAsync<TType>(string keyword,
            int userId,
            int page, int length,
            Expression<Func<FriendRequest, TType>> selector)
        {
            return await FindByCondition(e => e.AuthorId == userId && (e.Receiver.Name + " " + e.Receiver.Surname).Contains(keyword))
                .Skip((page - 1) * length)
                .Select(selector)
                .ToListAsync();
        }

        public async Task DeleteByEntity(FriendRequest friendRequest)
        {
            Delete(friendRequest);
            await SaveAsync();
        }

        public bool GetStatusByUserIds(int firstId, int secondId)
        {
            return ExistsByCondition(e =>
                    e.AuthorId == firstId && e.ReceiverId == secondId
                    || e.ReceiverId == firstId && e.AuthorId == secondId);
        }

        public async Task<IEnumerable<TType>> GetReceivedByUserIdAsync<TType>(int userId,
            Expression<Func<FriendRequest, TType>> selector)
        {
            return await FindByCondition(e => e.ReceiverId == userId)
                .Select(selector)
                .ToListAsync();
        }

        public async Task<IEnumerable<TType>> FilterReceivedByKeywordAndUserIdAsync<TType>(string keyword, 
            int userId,
            int page, int length,
            Expression<Func<FriendRequest, TType>> selector)
        {
            return await FindByCondition(e => e.ReceiverId == userId && (e.Author.Name+" "+e.Author.Surname).Contains(keyword)) 
                .Skip((page-1)*length)
                .Select(selector)
                .ToListAsync();
        }

        public async Task<IEnumerable<TType>> GetSentByUserIdAsync<TType>(int userId,
            Expression<Func<FriendRequest, TType>> selector)
        {
            return await FindByCondition(e => e.AuthorId == userId)
                .Select(selector)
                .ToListAsync();
        }

        
    }
}