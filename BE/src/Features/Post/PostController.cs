using System.Threading.Tasks;
using BE.Helpers;
using BE.Models;
using BE.Repositories;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BE.Features.Post
{
    [ApiController]
    [Route("api/post")]
    public class PostController : ControllerBase
    {
        private readonly IHubContext<PostHub> _hubContext;
        private readonly IRepositoryWrapper _repository;

        public PostController(IRepositoryWrapper repository,
            IHubContext<PostHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}/user-post")]
        public async Task<IActionResult> RemoveUserPostById([FromRoute] int id)
        {
            await RemovePostById(id);
            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [AuthorizeEventAdminInPostController]
        [Route("{id}/event-post/{eventId}")]
        public async Task<IActionResult> RemoveEventPostById([FromRoute] int id)
        {
            await RemovePostById(id);
            return Ok();
        }

        public async Task RemovePostById(int id)
        {
            await _repository.Post.RemoveByIdAsync(id);
        }

        [HttpPut]
        [Authorize]
        [Route("like/{id}/user-post")]
        public async Task<IActionResult> LikeUserPostById([FromRoute] int id,
            [FromHeader(Name = "userId")] int userId)
        {
            if (LikeExistsByPostId(id, userId)) return UnprocessableEntity();
            await LikePostById(id, userId);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("like/{id}/event-post/{eventId}")]
        public async Task<IActionResult> LikeEventPostById(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            if (LikeExistsByPostId(id, userId)) return UnprocessableEntity();
            await LikePostById(id, userId);
            return Ok();
        }

        private async Task LikePostById(int id, int userId)
        {
            var newLike = new PostLike
            {
                UserId = userId,
                PostId = id
            };
            await _repository.PostLike.CreateAsync(newLike);
        }

        [HttpPut]
        [Authorize]
        [Route("unlike/{id}/event-post/{eventId}")]
        public async Task<IActionResult> UnlikeEventPostById(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            if (!LikeExistsByPostId(id, userId)) return UnprocessableEntity();
            await UnlikePostById(id, userId);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("unlike/{id}/user-post")]
        public async Task<IActionResult> UnlikeUserPostById(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            if (!LikeExistsByPostId(id, userId)) return UnprocessableEntity();
            await UnlikePostById(id, userId);
            return Ok();
        }

        private async Task UnlikePostById(int id, int userId)
        {
            await _repository.PostLike.RemoveByPostIdAsync(id, userId);
        }

        private bool LikeExistsByPostId(int id, int userId)
        {
            return _repository.PostLike.GetPostLikedByUser(id, userId);
        }
    }
}