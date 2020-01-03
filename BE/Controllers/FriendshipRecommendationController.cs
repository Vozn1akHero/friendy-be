using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;
using BE.Repositories;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using RecommendationAlgorithm;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/friendship-recommendation")]
    public class FriendshipRecommendationController : ControllerBase
    {
        private readonly IFriendshipRecommendationService
            _friendshipRecommendationService;
        private readonly IRepositoryWrapper _repository;
        private readonly ICosSim _cosSim;
        
        public FriendshipRecommendationController(IFriendshipRecommendationService friendshipRecommendationService,
            IRepositoryWrapper repository, ICosSim cosSim)
        {
            _friendshipRecommendationService = friendshipRecommendationService;
            _repository = repository;
            _cosSim = cosSim;
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

        [HttpGet("recommendations")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromHeader(Name = "userId")] int userId)
        {
            bool refreshNeed = await _repository.FriendshipRecommendation
                .RefreshNeedByIssuerId(userId);
            if (refreshNeed)
            {
                var issuerInterests = await _repository.User
                    .GetInterestsById(userId);
                if (issuerInterests.Any())
                {
                    var recommendations = await _cosSim.CalculateAsync(userId, issuerInterests);
                    return Ok(recommendations);
                }
            }
            else
            {
                var recommendations = await _repository.FriendshipRecommendation
                    .FindPotentialFriendsByIssuerId(userId);
                return Ok(recommendations);
            }

            return UnprocessableEntity("INTEREST LIST IS EMPTY");
        }
    }
}