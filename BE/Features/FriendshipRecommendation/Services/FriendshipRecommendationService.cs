using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.FriendshipRecommendation
{
    public interface IFriendshipRecommendationService
    {
         IEnumerable<Models.FriendshipRecommendation> FindPotentialFriendsByIssuerId(int issuerId);
    }

    public class FriendshipRecommendationService : IFriendshipRecommendationService
    {
        private IRepositoryWrapper _repository;
        
        public FriendshipRecommendationService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        
        public IEnumerable<Models.FriendshipRecommendation> FindPotentialFriendsByIssuerId(int issuerId)
        {
            var recommendations = _repository.FriendshipRecommendation
                .FindPotentialFriendsByIssuerId(issuerId);
            return recommendations;
        }
    }
}