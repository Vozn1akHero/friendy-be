using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE;
using BE.Interfaces;
using BE.Models;
using BE.Repositories;

namespace RecommendationAlgorithm
{
    public interface ICosSim
    {
        Task<IEnumerable<FriendshipRecommendation>> CalculateAsync(int issuerId,
            IEnumerable<UserInterests> issuerInterests);
    }
    
    public class UserInterestsForCosSim
    {
        public int UserId { get; set; }
        public ICollection<UserInterests> UserInterests { get; set; }
    }
    public class CosSim : ICosSim
    {
        private IRepositoryWrapper _repository;
        
        public CosSim(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<FriendshipRecommendation>> CalculateAsync(int issuerId,
            IEnumerable<UserInterests> issuerInterests)
        {
            var allUsers = await _repository.User.GetAllWithInterestsAsync();
            var allInterests = allUsers.Where(e => e.Id != issuerId).Select(e => new
                UserInterestsForCosSim {UserId = e.Id, UserInterests = e
                .UserInterests}).ToList();
            var outputs = new List<Output>();
            foreach (var value in allInterests)
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
                .Take(5)
                .ToList();
            await AddRecommendationsByOutputsToDatabase(issuerId, mostSim);
            var friendshipsRecommendations = await _repository.FriendshipRecommendation
                .FindPotentialFriendsByIssuerId(issuerId);
            return friendshipsRecommendations;
        }

        private async Task AddRecommendationsByOutputsToDatabase(int issuerId, List<Output> outputs)
        {
            var friendshipRecommendations = new List<FriendshipRecommendation>();
            foreach (var value in outputs)
            {
                var friendshipStatus = await _repository.UserFriendship
                    .CheckIfFriendsByUserIdsAsync(issuerId, value.UserId);
                if (!friendshipStatus)
                {
                    friendshipRecommendations.Add(new FriendshipRecommendation
                    {
                        IssuerId = issuerId,
                        PotentialFriendId = value.UserId,
                        LastCheckupTime = DateTime.Now
                    });
                }
            }
            await _repository.FriendshipRecommendation.AddRange(
                friendshipRecommendations);
        }

        private static double CalculateForEntity(IEnumerable<UserInterests> issuerInterests,
            IEnumerable<UserInterests> otherUserInterests)
        {
            var wages = new List<List<int>>();
            foreach (var issuerInterest in issuerInterests)
            {
                var otherUserInterest = otherUserInterests.SingleOrDefault(e =>
                    e.InterestId == issuerInterest.InterestId);
                int otherUserInterestWage = otherUserInterest?.Wage ?? 0;
                int issuerInterestWage = issuerInterest.Wage;
                wages.Add(new List<int>{ issuerInterestWage, otherUserInterestWage });
            }
            double A = wages.Sum(e => e[0] * e[1]);
            double A1 = Math.Sqrt(wages.Sum(e => Math.Pow(e[0], 2)));
            double A2 = Math.Sqrt(wages.Sum(e => Math.Pow(e[1], 2)));
            double similarity = A / (A1 * A2);
            return similarity;
        }
    }
}