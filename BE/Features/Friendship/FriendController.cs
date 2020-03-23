using System.Threading.Tasks;
using BE.Features.Friendship.Dtos;
using BE.Features.Friendship.Helpers;
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

        [HttpGet("requests/received")]
        [Authorize]
        public async Task<IActionResult> GetReceivedRequests(
            [FromHeader(Name = "userId")] int userId)
        {
            var requests =
                await _repository.FriendRequest.GetReceivedByUserIdAsync(userId, ReceivedFriendRequestDto.Selector);
            return Ok(requests);
        }

        [HttpGet]
        [Authorize]
        [Route("requests/sent")]
        public async Task<IActionResult> GetSentRequests(
            [FromHeader(Name = "userId")] int userId)
        {
            var requests = await _repository.FriendRequest.GetSentByUserIdAsync(userId, SentFriendRequestDto.Selector);
            return Ok(requests);
        }
        
        [HttpPost]
        [Authorize]
        [Route("confirm/{id}")]
        public async Task<IActionResult> ConfirmRequest(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            try
            {
                await _userFriendshipService.ConfirmRequestAsync(userId, id);
                return Ok();
            }
            catch (FriendshipRequestNotFound)
            {
                return UnprocessableEntity("FRIEND REQUEST NOT FOUND");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}/paginate/{page}")]
        public async Task<IActionResult> GetUserFriendsWithPagination(int page, 
            int userId,
            [FromQuery(Name = "length")] int length,
            [FromQuery(Name = "userId")] int issuerId)
        {
            var friends = await _userFriendshipService.GetLastRangeByIdWithPaginationAsync(userId, page, length);
            return Ok(friends);
        }
        
        /*[HttpGet]
        [Authorize]
        [Route("paginate/logged-in/{page}")]
        public async Task<IActionResult> GetLoggedInUserFriendsWithPagination(int page,
            [FromQuery(Name = "length")] int length,
            [FromHeader(Name = "userId")] int userId)
        {
            var friends =
                await _userFriendshipService.GetLastRangeByIdWithPaginationAsync(userId,
                    page, length);

            return Ok(friends);
        }*/

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
        public async Task<IActionResult> RemoveByIdAsync(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            await _repository.UserFriendship.RemoveByIdentifiersAsync(id, userId);
            await _repository.SaveAsync();
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
    }
}