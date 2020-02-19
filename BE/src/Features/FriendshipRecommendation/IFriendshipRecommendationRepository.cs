using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Repositories;

namespace BE.Features.FriendshipRecommendation
{
    public interface IFriendshipRecommendationRepository
        : IRepositoryBase<Models.FriendshipRecommendation>
    {
        Task Add(Models.FriendshipRecommendation friendshipRecommendation);
        Task AddRange(IEnumerable<Models.FriendshipRecommendation> friendshipRecommendations);

        Task<IEnumerable<Models.FriendshipRecommendation>>
            FindPotentialFriendsByIssuerId(int id);

        Task<bool> RefreshNeedByIssuerId(int id);
    }
}