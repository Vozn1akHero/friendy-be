using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BE.Controllers
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
        [Route("{id}")]
        public async Task<IActionResult> RemoveById([FromRoute] int id)
        {
            await _repository.Post.RemoveByIdAsync(id);
            return Ok();
        }
        
        [HttpPut]
        [Authorize]
        [Route("like/{id}")]
        public async Task<IActionResult> LikeUserPostById([FromRoute] int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var newLike = new PostLike
            {
                UserId = userId,
                PostId = id
            };
            await _repository.PostLike.CreateAsync(newLike);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("unlike/{id}")]
        public async Task<IActionResult> UnlikePostById(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            await _repository.PostLike.RemoveByPostIdAsync(id, userId);
            return Ok();
        }

        [HttpGet("liked-by-user/{userId}")]
        [Authorize]
        public IActionResult GetLikedByUser(int postId,
            [FromHeader(Name = "userId")] int userId)
        {
            bool status = _repository.PostLike.GetPostLikedByUser(postId, userId);
            return Ok(status);
        }
    }
}