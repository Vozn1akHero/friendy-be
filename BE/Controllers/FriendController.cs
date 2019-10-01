using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Dtos.FriendDtos;
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
       // public IAppCache _cache;
        
        public FriendController(IRepositoryWrapper repositoryWrapper,
            IJwtService jwtService)
        {
            _repositoryWrapper = repositoryWrapper;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Authorize]
        [Route("addNew/{id}")]
        public async Task<IActionResult> AddNew(int id, [FromHeader(Name = "userId")] int userId)
        {
            await _repositoryWrapper.UserFriends.AddNew(id, userId);
            var newChat = await _repositoryWrapper.Chat.AddNewAfterFriendAdding();
            await _repositoryWrapper.ChatParticipants.AddNewAfterFriendAdding(newChat.Id, new []{ id, userId });
            return Ok();
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

        [HttpGet]
        [Authorize]
        [Route("getExemplaryByUserId")]
        public async Task<IActionResult> GetExemplaryByUserId([FromHeader(Name = "userId")] int userId)
        {
            List<ExemplaryFriendDto> exemplaryFriendsDtos
                = new List<ExemplaryFriendDto>();
            
            var exemplaryFriends = await _repositoryWrapper.UserFriends.GetExemplaryByUserId(userId);
            
            exemplaryFriends.ForEach(exemplaryFriend =>
            {
                var content = new FileStream(exemplaryFriend.Friend.FriendNavigation.Avatar, FileMode.Open, FileAccess.Read, FileShare.Read);
                var response = File(content, "application/octet-stream");

                exemplaryFriendsDtos.Add(new ExemplaryFriendDto
                {
                    Id = exemplaryFriend.FriendId,
                    Avatar = response
                });
            });
            
            return Ok(exemplaryFriendsDtos);
        }
    }
}