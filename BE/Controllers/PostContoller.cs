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
    public class PostContoller : ControllerBase
    {
        private readonly IHubContext<PostHub> _hubContext;
        private readonly IRepositoryWrapper _repository;
        
        public PostContoller(IRepositoryWrapper repository, 
            IHubContext<PostHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
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
            return Ok(newLike);
        }

        [HttpPut]
        [Authorize]
        [Route("unlike/{postId}")]
        public async Task<IActionResult> UnlikePostById(int postId,
            [FromHeader(Name = "userId")] int userId)
        {
            await _repository.PostLike.RemoveByPostIdAsync(postId, userId);
            return Ok();
        }
    }
}