using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/friend")]
    public class FriendController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IJwtService _jwtService;
        public IAppCache _cache;
        
        public FriendController(IRepositoryWrapper repositoryWrapper, IJwtService jwtService)
        {
            _repositoryWrapper = repositoryWrapper;
            _jwtService = jwtService;
            _cache = new CachingService();
        }

        [HttpGet]
        [Authorize]
        [Route("getExampleFriends")]
        public async Task<IActionResult> GetFriends([FromQuery(Name = "startIndex")] int startIndex, 
            [FromQuery(Name = "lastIndex")] int lastIndex, [FromHeader(Name = "userId")] int userId)
        {
            //int userId = _jwtService.GetUserIdFromJwt(HttpContext.Request.Cookies["SESSION_TOKEN"]);
            var friends = await _repositoryWrapper.UserFriends.GetByUserId(userId, startIndex, lastIndex);
            return Ok(friends);
        }

        [HttpGet]
        [Authorize]
        [Route("filterFriends")]
        public async Task<IActionResult> FilterFriends([FromQuery(Name = "keyword")] string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var friends = await _repositoryWrapper.UserFriends.FilterByKeyword(userId, keyword);
            return Ok(friends);
        }

        [HttpDelete]
        [Authorize]
        [Route("remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _repositoryWrapper.UserFriends.RemoveById(id);
            return Ok();
        }
        
    }
}