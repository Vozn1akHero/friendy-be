using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Repositories;

namespace BE.Features.FriendshipRecommendation
{
    public interface IFriendshipRecommendationRepository
        : IRepositoryBase<Models.FriendshipRecommendation>
    {
        Task Add(Models.FriendshipRecommendation friendshipRecommendation);
        void DeleteByIssuerId(int id);
        Task AddRange(IEnumerable<Models.FriendshipRecommendation> friendshipRecommendations);

        IEnumerable<Models.FriendshipRecommendation>
            FindPotentialFriendsByIssuerId(int id);

        bool RefreshNeedByIssuerId(int id);
    }
}