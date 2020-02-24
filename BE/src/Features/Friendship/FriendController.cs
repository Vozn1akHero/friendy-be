using System.Threading.Tasks;
using BE.Features.Friendship.Services;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Friendship
{
    [ApiController]
    [Route("api/friend")]
    public class FriendController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IUserFriendshipService _userFriendshipService;

        public FriendController(IRepositoryWrapper repository,
            IUserFriendshipService userFriendshipService)
        {
            _repository = repository;
            _userFriendshipService = userFriendshipService;
        }

        [HttpGet("status/{receiverId}")]
        [Authorize]
        public async Task<IActionResult> GetStatus(int receiverId,
            [FromHeader(Name = "userId")] int userId)
        {
            var status =
                await _userFriendshipService.GetFriendshipStatus(receiverId, userId);
            return Ok(status);
        }

        [HttpPost]
        [Authorize]
        [Route("request/{receiverId}")]
        public async Task<IActionResult> AddNewRequest(int receiverId,
            [FromHeader(Name = "userId")] int userId)
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
            var friendRequest =
                await _repository.FriendRequest.FindByUserIds(userId, receiverId);
            if (friendRequest != null)
                await _repository.FriendRequest.DeleteByEntity(friendRequest);
            else
                return UnprocessableEntity();
            return Ok();
        }

        /*[HttpGet]
        [Authorize]
        [Route("request/status")]
        public async Task<IActionResult> GetReceivedRequestStatus(int receiverId,
            [FromHeader(Name = "userId")] int userId)
        {
            var status =
                await _repository.FriendRequest.GetStatusByUserIds(receiverId, userId);
            return Ok(status);
        }*/

        [HttpGet("requests/received")]
        [Authorize]
        public async Task<IActionResult> GetReceivedRequests(
            [FromHeader(Name = "userId")] int userId)
        {
            var requests =
                await _repository.FriendRequest.GetReceivedByUserIdWithDto(userId);
            return Ok(requests);
        }

        [HttpGet]
        [Authorize]
        [Route("requests/sent")]
        public async Task<IActionResult> GetSentRequests(
            [FromHeader(Name = "userId")] int userId)
        {
            var requests = await _repository.FriendRequest.GetSentByUserIdWithDto(userId);
            return Ok(requests);
        }


        [HttpPost]
        [Authorize]
        [Route("confirm/{id}")]
        public async Task<IActionResult> ConfirmRequest(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            await _repository.UserFriendship.AddNewAsync(id, userId);
            await _repository.Chat.Add(id, userId);
            return Ok();
        }

        /*[HttpGet]
        [Authorize]
        [Route("range/logged-in")]
        public async Task<IActionResult> GetLoggedInUserFriendsRange(
            [FromQuery(Name = "startIndex")] int startIndex,
            [FromQuery(Name = "length")] int length,
            [FromHeader(Name = "userId")] int userId)
        {
            var friends = await _userFriendshipService
                .GetRangeByUserIdAsync(userId, startIndex, length);
            return Ok(friends);
        }*/

        [HttpGet]
        [Authorize]
        [Route("paginate/{userId}")]
        public async Task<IActionResult> GetUserFriendsWithPagination([FromQuery(Name = "page")] int page, 
        int userId)
        {
            var friends =
                await _userFriendshipService.GetLastRangeByIdWithPaginationAsync(userId,
                    page);

            return Ok(friends);
        }
        
        [HttpGet]
        [Authorize]
        [Route("paginate/logged-in/{page}")]
        public async Task<IActionResult> GetLoggedInUserFriendsWithPagination(int page,
            [FromHeader(Name = "userId")] int userId)
        {
            var friends =
                await _userFriendshipService.GetLastRangeByIdWithPaginationAsync(userId,
                    page);

            return Ok(friends);
        }

        [HttpGet]
        [Authorize]
        [Route("filter")]
        public async Task<IActionResult> FilterFriends(
            [FromQuery(Name = "keyword")] string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var friends =
                await _userFriendshipService.FilterByKeywordAsync(userId, keyword);
            return Ok(friends);
        }

        [HttpDelete]
        [Authorize]
        [Route("remove/{id}")]
        public async Task<IActionResult> Remove(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            await _repository.UserFriendship.RemoveByIdentifiersAsync(id, userId);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("recommended")]
        public async Task<IActionResult> GetRecommended(
            [FromQuery(Name = "firstIndex")] int firstIndex,
            [FromQuery(Name = "lastIndex")] int lastIndex)
        {
            var users = await _repository.User.GetByRangeAsync(firstIndex, lastIndex);
            return Ok(users);
        }

        /*[HttpGet]
        [Authorize]
        [Route("exemplary/logged-in")]
        public async Task<IActionResult> GetExemplaryLoggedInUser(
            [FromHeader(Name = "userId")] int userId)
        {
            var exemplaryFriends = await _userFriendshipService
                .GetExemplaryByUserIdAsync(userId);

            return Ok(exemplaryFriends);
        }

        [HttpGet]
        [Authorize]
        [Route("exemplary/{userId}")]
        public async Task<IActionResult> GetExemplaryByUserId(int userId)
        {
            var exemplaryFriends = await _userFriendshipService
                .GetExemplaryByUserIdAsync(userId);

            return Ok(exemplaryFriends);
        }*/

        /*[HttpGet]
        [Authorize]
        [Route("friendship-status")]
        public async Task<IActionResult> GetFriendshipStatus(
            [FromQuery(Name = "id")] int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var friendshipStatus =
                await _repository.UserFriendship.CheckIfFriendsByUserIdsAsync(id, userId);
            return Ok(friendshipStatus);
        }*/
    }
}