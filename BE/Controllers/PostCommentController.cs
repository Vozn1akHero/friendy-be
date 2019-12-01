using System.Threading.Tasks;
using BE.Dtos.CommentDtos;
using BE.Interfaces;
using BE.Models;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/post-comment")]
    public class PostCommentController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IHubContext<PostHub> _hubContext;
        
        public PostCommentController(IRepositoryWrapper repository, 
            IHubContext<PostHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        [HttpGet("range/{postId}")]
        [Authorize]
        public async Task<IActionResult> GetAllByPostId(int postId,
            [FromQuery(Name = "startIndex")] int startIndex,
            [FromQuery(Name = "length")] int length,
            [FromHeader(Name = "userId")] int userId)
        {
            var posts = await _repository.Comment.GetRangeByPostIdAuthedAsync(postId, startIndex, length, userId);
            return Ok(posts);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetAllByPostId([FromHeader(Name = "userId")] int userId, [FromBody] NewCommentDto newCommentDto)
        {
            var newComment = new Comment
            {
                PostId = newCommentDto.PostId,
                Content = newCommentDto.Content,
                UserId = userId
            };
            await _repository.Comment.AddAsync(newComment);
            return Ok();
        }
        
    }
}