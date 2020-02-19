using BE.Models;

namespace BE.Features.FriendshipRecommendation
{
    public interface IFriendshipRecommendationService
    {
        /*Task CreateVisitAsync(int visitedProfileId, int visitorId);

        Task CreateSearchDataAsync(int issuerId, string name,
            string surname);*/
    }

    public class FriendshipRecommendationService : IFriendshipRecommendationService
    {
        private readonly FriendyContext _friendyContext;

        public FriendshipRecommendationService(FriendyContext friendyContext)
        {
            _friendyContext = friendyContext;
        }

/*        public async Task CreateVisitAsync(int visitedProfileId, int visitorId)
        {
            _friendyContext.RecVisitedProfile.Add(new RecVisitedProfile()
            {
                VisitedUserProfileId = visitedProfileId,
                UserId = visitorId
            });
            await _friendyContext.SaveChangesAsync();
        }

        public async Task CreateSearchDataAsync(int issuerId, string name,
            string surname)
        {
            _friendyContext.BasicSearchHistory.Add(new BasicSearchHistory()
            {
                UserId = issuerId,
                InsertedUserName = name,
                InsertedUserSurname = surname
            });
            await _friendyContext.SaveChangesAsync();
        }*/
    }
}