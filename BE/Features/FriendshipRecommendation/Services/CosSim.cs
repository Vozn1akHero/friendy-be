using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.FriendshipRecommendation
{
    public interface ICosSim
    {
        bool CheckIfRefreshIsNeeded(int issuerId);
        Task DeleteEntitiesForUser(int issuerId);
        Task<IEnumerable<Output>> CalculateAsync(int issuerId);

        Task AddRecommendationsByOutputsToDatabase(int issuerId,
            IEnumerable<Output> outputs);
    }

    public class UserInterestsForCosSim
    {
        public int UserId { get; set; }
        public ICollection<UserInterests> UserInterests { get; set; }
    }

    public class CosSim : ICosSim
    {
        private readonly IRepositoryWrapper _repository;
        private int _outputSize = 5;

        public CosSim(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public bool CheckIfRefreshIsNeeded(int issuerId)
        {
            bool refreshNeed = _repository.FriendshipRecommendation
                .RefreshNeedByIssuerId(issuerId);
            return refreshNeed;
        }

        public async Task DeleteEntitiesForUser(int issuerId)
        {
            _repository.FriendshipRecommendation.DeleteByIssuerId(issuerId);
            await _repository.SaveAsync();
        }
        
        public async Task<IEnumerable<Output>> CalculateAsync(int issuerId)
        {
            var issuerInterests = await _repository.User.GetInterestsByIdAsync(issuerId);
            var allUsersWithInterests = await _repository.User.GetAllAsync(e => new UserInterestsForCosSim
            {
                UserId = e.Id,
                UserInterests = e.UserInterests
            });
            var allInterestsExceptIssuer = allUsersWithInterests.Where(e => e.UserId != issuerId);
            var outputs = new List<Output>();
            foreach (var value in allInterestsExceptIssuer)
            {
                var output = new Output
                {
                    IssuerId = issuerId,
                    UserId = value.UserId,
                    SimilarityScore = CalculateForEntity(issuerInterests, value.UserInterests)
                };
                outputs.Add(output);
            }
            var mostSim = outputs
                .OrderByDescending(e => e.SimilarityScore)
                .Take(_outputSize)
                .ToList();
            return mostSim;
        }

        public async Task AddRecommendationsByOutputsToDatabase(int issuerId,
            IEnumerable<Output> outputs)
        {
            var friendshipRecommendations = new List<Models.FriendshipRecommendation>();
            foreach (var value in outputs)
            {
                var friendshipStatus = await _repository.UserFriendship
                    .CheckIfFriendsByUserIdsAsync(issuerId, value.UserId);
                if (!friendshipStatus)
                    friendshipRecommendations.Add(new Models.FriendshipRecommendation
                    {
                        IssuerId = issuerId,
                        PotentialFriendId = value.UserId,
                        LastCheckupTime = DateTime.Now
                    });
            }
            await _repository.FriendshipRecommendation.AddRange(
                friendshipRecommendations);
        }

        private static double CalculateForEntity(
            IEnumerable<UserInterests> issuerInterests,
            IEnumerable<UserInterests> otherUserInterests)
        {
            var wages = new List<List<int>>();
            foreach (var issuerInterest in issuerInterests)
            {
                var otherUserInterest = otherUserInterests.SingleOrDefault(e =>
                    e.InterestId == issuerInterest.InterestId);
                var otherUserInterestWage = otherUserInterest?.Wage ?? 0;
                int issuerInterestWage = issuerInterest.Wage;
                wages.Add(new List<int> {issuerInterestWage, otherUserInterestWage});
            }

            double A = wages.Sum(e => e[0] * e[1]);
            var A1 = Math.Sqrt(wages.Sum(e => Math.Pow(e[0], 2)));
            var A2 = Math.Sqrt(wages.Sum(e => Math.Pow(e[1], 2)));
            var similarity = A / (A1 * A2);
            return similarity;
        }
    }
}