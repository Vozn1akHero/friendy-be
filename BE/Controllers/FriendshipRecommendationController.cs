using System.Threading.Tasks;
using BE.Interfaces;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/friendship-recommendation")]
    public class FriendshipRecommendationController : ControllerBase
    {
        private readonly IFriendshipRecommendationService
            _friendshipRecommendationService;

        public FriendshipRecommendationController(IFriendshipRecommendationService friendshipRecommendationService)
        {
            _friendshipRecommendationService = friendshipRecommendationService;
        }

        [HttpPost("profile-visit/{profileId}")]
        [Authorize]
        public async Task<IActionResult> SetPageVisitedAsync(int profileId,
            [FromHeader(Name = "userId")] int userId)
        {
            //await _repository.
            await _friendshipRecommendationService.CreateVisitAsync(profileId,
                userId);
            return Ok();
        }
        
        [HttpPost("basic-search-data")]
        [Authorize]
        public async Task<IActionResult> CreateSearchData([FromQuery(Name = "insertedName")] string name,
            [FromQuery(Name = "insertedSurname")] string surname,
            [FromHeader(Name = "userId")] int userId)
        {
            //await _repository.
            await _friendshipRecommendationService.CreateSearchDataAsync(userId,
                name, surname);
            return Ok();
        }
    }
}