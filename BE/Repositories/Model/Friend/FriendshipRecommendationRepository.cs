using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class FriendshipRecommendationRepository : 
    RepositoryBase<FriendshipRecommendation>, IFriendshipRecommendationRepository
    {
        public FriendshipRecommendationRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task Add(FriendshipRecommendation friendshipRecommendation)
        {
            Create(friendshipRecommendation);
            await SaveAsync();
        }

        public async Task<IEnumerable<FriendshipRecommendation>> FindPotentialFriendsByIssuerId(int id)
        {
            return await FindByCondition(e => e.IssuerId == id)
                .Include(e => e.PotentialFriend)
                .ToListAsync();
        }

        public async Task AddRange(
            IEnumerable<FriendshipRecommendation> friendshipRecommendations)
        {
            CreateAll(friendshipRecommendations);
            await SaveAsync();
        }
        
        public async Task<bool> RefreshNeedByIssuerId(int id)
        {
            var entity = await FindByCondition(e => e.IssuerId == id)
                .Take(1)
                .SingleOrDefaultAsync();
            if (entity != null)
            {
                bool res = DateTime.Now > entity.LastCheckupTime.Date.AddDays(1);
                DeleteAll(e => e.IssuerId == id);
                return res;
            }
            return true;
        }
    }
}