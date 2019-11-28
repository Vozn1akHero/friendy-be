using System.Threading.Tasks;
using BE.Interfaces;
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
        [Route("request/{receiverId}")]
        public async Task<IActionResult> AddNewRequest(int receiverId, [FromHeader(Name = "userId")] int userId)
        {
            //var receiverFriend = await _repository.Friend.GetByUserId(receiverId); 
            await _repository.FriendRequest.Add(userId, receiverId);
            return Ok();
        }
        
        [HttpDelete]
        [Authorize]
        [Route("request/{receiverId}")]
        public async Task<IActionResult> DeleteRequest(int receiverId,
            [FromHeader(Name = "userId")] int userId)
        {
            //var receiverFriend = await _repository.Friend.GetByUserId(receiverId);
            var friendRequest = await _repository.FriendRequest.FindByUserIds(userId, receiverId);
            if (friendRequest != null)
            {
                await _repository.FriendRequest.DeleteByEntity(friendRequest);
            }
            else
            {
                return UnprocessableEntity();
            }
            return Ok();
        }
        
        [HttpGet]
        [Authorize]
        [Route("request/status")]
        public async Task<IActionResult> GetReceivedRequestStatus(int receiverId, 
            [FromHeader(Name = "userId")] int userId)
        {
            //var receiverFriend = await _repository.Friend.GetByUserId(receiverId);
            bool status = await _repository.FriendRequest.GetStatusByUserIds(receiverId, userId);
            return Ok(status);
        }
        
        [HttpGet("requests/received")]
        [Authorize]
        public async Task<IActionResult> GetReceivedRequests([FromHeader(Name = "userId")] int userId)
        {
            var requests = await _repository.FriendRequest.GetReceivedByUserIdWithDto(userId);
            return Ok(requests);
        }

        [HttpGet]
        [Authorize]
        [Route("requests/sent")]
        public async Task<IActionResult> GetSentRequests([FromHeader(Name = "userId")] int userId)
        {
            var requests = await _repository.FriendRequest.GetSentByUserIdWithDto(userId);
            return Ok(requests);
        }
        
        /*
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
        */

        [HttpPost]
        [Authorize]
        [Route("confirm/{id}")]
        public async Task<IActionResult> ConfirmRequest(int id, [FromHeader(Name = "userId")] int userId)
        {
            await _repository.UserFriendship.AddNewAsync(id, userId);
            await _repository.Chat.Add(id, userId);
            //await _repository.ChatParticipants.AddNewAfterFriendAdding(newChat.Id, new[] {id, userId});
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("range/logged-in")]
        public async Task<IActionResult> GetLoggedInUserFriendsRange([FromQuery(Name = "startIndex")] int startIndex,
            [FromQuery(Name = "length")] int length, [FromHeader(Name = "userId")] int userId)
        {
            var friends = await _repository
                .UserFriendship.GetRangeByUserIdAsync(userId, startIndex, length);

            return Ok(friends);
        }

        [HttpGet]
        [Authorize]
        [Route("filter")]
        public async Task<IActionResult> FilterFriends([FromQuery(Name = "keyword")] string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var friends = await _repository.UserFriendship.FilterByKeywordAsync(userId, keyword);
            return Ok(friends);
        }

        [HttpDelete]
        [Authorize]
        [Route("remove/{id}")]
        public async Task<IActionResult> Remove(int id, [FromHeader(Name = "userId")] int userId)
        {
            await _repository.UserFriendship.RemoveByIdentifiersAsync(id, userId);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("recommended")]
        public async Task<IActionResult> GetRecommended([FromQuery(Name = "firstIndex")] int firstIndex,
            [FromQuery(Name = "lastIndex")] int lastIndex)
        {
            var users = await _repository.User.GetByRangeAsync(firstIndex, lastIndex);
            return Ok(users);
        }

        [HttpGet]
        [Authorize]
        [Route("exemplary/logged-in")]
        public async Task<IActionResult> GetExemplaryLoggedInUser([FromHeader(Name = "userId")] int userId)
        {
            var exemplaryFriends = await _repository
                .UserFriendship
                .GetExemplaryByUserIdAsync(userId);

            return Ok(exemplaryFriends);
        }
        
        [HttpGet]
        [Authorize]
        [Route("exemplary/{userId}")]
        public async Task<IActionResult> GetExemplaryByUserId(int userId)
        {
            var exemplaryFriends = await _repository
                .UserFriendship
                .GetExemplaryByUserIdAsync(userId);

            return Ok(exemplaryFriends);
        }

        [HttpGet]
        [Authorize]
        [Route("friendship-status")]
        public async Task<IActionResult> GetFriendshipStatus([FromQuery(Name = "id")] int id,
            [FromHeader(Name = "userId")] int userId)
        {
            bool friendshipStatus = await _repository.UserFriendship.CheckIfFriendsByUserIdsAsync(id, userId);
            return Ok(friendshipStatus);
        }
    }
}