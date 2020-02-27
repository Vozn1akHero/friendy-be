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

        public void DeleteByIssuerId(int id)
        {
            DeleteAll(e => e.IssuerId == id);
        }

        public IEnumerable<Models.FriendshipRecommendation>
            FindPotentialFriendsByIssuerId(int id)
        {
            return FindByCondition(e => e.IssuerId == id)
                .Include(e => e.Issuer)
                .Include(e => e.PotentialFriend);
        }

        public async Task AddRange(
            IEnumerable<Models.FriendshipRecommendation> friendshipRecommendations)
        {
            CreateAll(friendshipRecommendations);
            await SaveAsync();
        }

        public bool RefreshNeedByIssuerId(int id)
        {
            var entity = FindByCondition(e => e.IssuerId == id)
                .Take(1)
                .SingleOrDefault();
            if (entity != null)
            {
                return DateTime.Now > entity.LastCheckupTime.Date.AddDays(1);
            }

            return true;
        }
    }
}