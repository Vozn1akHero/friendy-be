using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.FriendshipRecommendation
{
    public class FriendshipRecommendationRepository :
        RepositoryBase<Models.FriendshipRecommendation>, IFriendshipRecommendationRepository
    {
        public FriendshipRecommendationRepository(FriendyContext friendyContext) : base(
            friendyContext)
        {
        }

        public async Task Add(Models.FriendshipRecommendation friendshipRecommendation)
        {
            Create(friendshipRecommendation);
            await SaveAsync();
        }

        public async Task<IEnumerable<Models.FriendshipRecommendation>>
            FindPotentialFriendsByIssuerId(int id)
        {
            return await FindByCondition(e => e.IssuerId == id)
                .Include(e => e.PotentialFriend)
                .ToListAsync();
        }

        public async Task AddRange(
            IEnumerable<Models.FriendshipRecommendation> friendshipRecommendations)
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
                var res = DateTime.Now > entity.LastCheckupTime.Date.AddDays(1);
                DeleteAll(e => e.IssuerId == id);
                return res;
            }

            return true;
        }
    }
}