using System.Threading.Tasks;
using BE.Features.Comment.Dto;
using BE.Features.Comment.Services;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BE.Features.Comment
{
    [ApiController]
    [Route("api/post-comment")]
    public class PostCommentController : ControllerBase
    {
        private readonly IHubContext<PostHub> _hubContext;
        private readonly IPostCommentService _postCommentService;

        public PostCommentController(IHubContext<PostHub> hubContext,
            IPostCommentService postCommentService)
        {
            _hubContext = hubContext;
            _postCommentService = postCommentService;
        }

        [HttpPost("like/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Like(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var createdEntity = await _postCommentService.Like(id, userId);
            return CreatedAtAction("Like", createdEntity);
        }

        [HttpDelete("unlike/{id}")]
        [Authorize]
        public async Task<IActionResult> Unlike(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var removedEntity = await _postCommentService.Unlike(id, userId);
            return Ok(removedEntity);
        }

        [HttpGet("all/{postId}")]
        [Authorize]
        public async Task<IActionResult> GetAllByPostId(int postId,
            [FromHeader(Name = "userId")] int userId)
        {
            var posts = await _postCommentService.GetAllMainByPostIdAsync(postId, userId);
            return Ok(posts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(
            [FromHeader(Name = "userId")] int userId,
            [FromBody] NewCommentDto newCommentDto,
            [FromForm] IFormFile file)
        {
            var entity = await _postCommentService.CreateAndReturnMainComment(
                newCommentDto,
                userId, file);
            return CreatedAtAction("Create", entity);
        }
    }
}