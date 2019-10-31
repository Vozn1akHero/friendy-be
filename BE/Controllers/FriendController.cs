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
        private readonly IRepositoryWrapper _repository;
        private readonly IJwtService _jwtService;
       // public IAppCache _cache;
        
        public FriendController(IRepositoryWrapper repository,
            IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Authorize]
        [Route("addNew/{id}")]
        public async Task<IActionResult> AddNew(int id, [FromHeader(Name = "userId")] int userId)
        {
            await _repository.UserFriends.AddNew(id, userId);
            var newChat = await _repository.Chat.AddNewAfterFriendAdding();
            await _repository.ChatParticipants.AddNewAfterFriendAdding(newChat.Id, new []{ id, userId });
            return Ok();
        }
        
        [HttpGet]
        [Authorize]
        [Route("sample/indexed")]
        public async Task<IActionResult> GetFriends([FromQuery(Name = "startIndex")] int startIndex, 
            [FromQuery(Name = "lastIndex")] int lastIndex, [FromHeader(Name = "userId")] int userId)
        {
            var shortenedFriends = await _repository
                .UserFriends.GetIndexedShortenedByUserId(userId, startIndex, lastIndex);
            var friends = new List<FriendDto>();
            foreach (var shortenedFriend in shortenedFriends)
            {
                using (FileStream fs = new FileStream(shortenedFriend.AvatarPath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(shortenedFriend.AvatarPath);
                    fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                    var friend = new FriendDto
                    {
                        Id = shortenedFriend.Id,
                        Name = shortenedFriend.Name,
                        Surname = shortenedFriend.Surname,
                        DialogLink = "da2jkd21l34",
                        OnlineStatus = true,
                        Avatar = bytes
                    };
                    friends.Add(friend);
                    fs.Close();
                }
            }
            
            return Ok(friends);
        }

        [HttpGet]
        [Authorize]
        [Route("filterFriends")]
        public async Task<IActionResult> FilterFriends([FromQuery(Name = "keyword")] string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var friends = await _repository.UserFriends.FilterByKeyword(userId, keyword);
            return Ok(friends);
        }

        [HttpDelete]
        [Authorize]
        [Route("remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _repository.UserFriends.RemoveById(id);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("getExemplaryByUserId")]
        public async Task<IActionResult> GetExemplaryByUserId([FromHeader(Name = "userId")] int userId)
        {
            List<ExemplaryFriendDto> exemplaryFriendsDtos
                = new List<ExemplaryFriendDto>();
            
            var exemplaryFriends = await _repository
                .UserFriends
                .GetExemplaryByUserId(userId);
            
            exemplaryFriends.ForEach(exemplaryFriend =>
            {
                using (FileStream fs = new FileStream(exemplaryFriend.Friend.FriendNavigation.Avatar, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(exemplaryFriend.Friend.FriendNavigation.Avatar);
                    fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                    exemplaryFriendsDtos.Add(new ExemplaryFriendDto
                    {
                        Id = exemplaryFriend.FriendId,
                        Avatar = bytes
                    });
                    fs.Close();
                }
            });
            
            return Ok(exemplaryFriendsDtos);
        }
    }
}