using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IFriendshipRecommendationRepository 
        : IRepositoryBase<FriendshipRecommendation>
    {
        Task Add(FriendshipRecommendation friendshipRecommendation);

        Task<IEnumerable<FriendshipRecommendation>>
            FindPotentialFriendsByIssuerId(int id);

        Task<bool> RefreshNeedByIssuerId(int id);
    }
}