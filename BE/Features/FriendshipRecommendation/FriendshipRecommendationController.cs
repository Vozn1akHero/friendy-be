using System.Linq;
using System.Threading.Tasks;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.FriendshipRecommendation
{
    [ApiController]
    [Route("api/friendship-recommendation")]
    public class FriendshipRecommendationController : ControllerBase
    {
        private readonly IFriendshipRecommendationService _friendshipRecommendationService;

        public FriendshipRecommendationController(IFriendshipRecommendationService friendshipRecommendationService)
        {
            _friendshipRecommendationService = friendshipRecommendationService;
        }


        [HttpGet("recommendations")]
        [Authorize]
        public IActionResult GetAll([FromHeader(Name = "userId")] int userId)
        {
            var recommendations = _friendshipRecommendationService.FindPotentialFriendsByIssuerId(userId);
            return Ok(recommendations);
        }
    }
}