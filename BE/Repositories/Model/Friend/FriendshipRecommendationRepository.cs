using System;
using System.Collections;
using System.Collections.Generic;
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

        public async Task<bool> RefreshNeedByIssuerId(int id)
        {
            var entity = await FindByCondition(e => e.IssuerId == id)
                .SingleOrDefaultAsync();
            if (entity != null)
            {
                bool res = DateTime.Now > entity.LastCheckupTime.Date.AddDays(1);
                //....
                return res;
            }
            return true;
        }
    }
}