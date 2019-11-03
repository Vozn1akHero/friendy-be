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

        public FriendController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize]
        [Route("request/{id}")]
        public async Task<IActionResult> AddNewRequest(int id, [FromHeader(Name = "userId")] int userId)
        {
            await _repository.FriendRequest.Add(userId, id);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("requests/received")]
        public async Task<IActionResult> GetReceivedRequests([FromHeader(Name = "userId")] int userId)
        {
            var requests = await _repository.FriendRequest.GetReceivedByUserId(userId);
            return Ok(requests);
        }

        [HttpGet]
        [Authorize]
        [Route("requests/sent")]
        public async Task<IActionResult> GetSentRequests([FromHeader(Name = "userId")] int userId)
        {
            var requests = await _repository.FriendRequest.GetSentByUserId(userId);
            return Ok(requests);
        }

        [HttpPost]
        [Authorize]
        [Route("confirm/{id}")]
        public async Task<IActionResult> ConfirmRequest(int id, [FromHeader(Name = "userId")] int userId)
        {
            await _repository.UserFriends.AddNew(id, userId);
            var newChat = await _repository.Chat.AddNewAfterFriendAdding();
            await _repository.ChatParticipants.AddNewAfterFriendAdding(newChat.Id, new[] {id, userId});
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
        [Route("filter")]
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
        [Route("recommended")]
        public async Task<IActionResult> GetRecommended([FromQuery(Name = "firstIndex")] int firstIndex,
            [FromQuery(Name = "lastIndex")] int lastIndex)
        {
            var users = await _repository.User.GetByRange(firstIndex, lastIndex);
            return Ok(users);
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
                using (FileStream fs = new FileStream(exemplaryFriend.Friend.FriendNavigation.Avatar, FileMode.Open,
                    FileAccess.Read))
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

        [HttpGet]
        [Authorize]
        [Route("friendship-status")]
        public async Task<IActionResult> GetFriendshipStatus([FromQuery(Name = "id")] int id,
            [FromHeader(Name = "userId")] int userId)
        {
            bool friendshipStatus = _repository.UserFriends.CheckIfFriendsByUserIds(id, userId);
            return Ok(friendshipStatus);
        }
    }
}