using System.Threading.Tasks;
using BE.Dtos.CommentDtos;
using BE.Interfaces;
using BE.Models;
using BE.Services.Model;
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
        private readonly IHubContext<PostHub> _hubContext;
        private readonly IRepositoryWrapper _repository;
        private IPostCommentService _postCommentService;

        public PostCommentController(IRepositoryWrapper repository,
            IHubContext<PostHub> hubContext, IPostCommentService postCommentService)
        {
            _repository = repository;
            _hubContext = hubContext;
            _postCommentService = postCommentService;
        }

        [HttpGet("all/{postId}")]
        [Authorize]
        public async Task<IActionResult> GetAllByPostId(int postId,
            [FromHeader(Name = "userId")] int userId)
        {
            var posts =
                await _postCommentService.GetAllMainByPostIdAuthedAsync(postId, userId);
            return Ok(posts);
        }
        
        [HttpGet("response/all/{commentId}")]
        [Authorize]
        public async Task<IActionResult> GetAllResponsesByCommentIdAsync(int commentId,
            [FromHeader(Name = "userId")] int userId)
        {
            var responses =
                await _postCommentService.GetAllCommentResponsesAsync(commentId, userId);
            return Ok(responses);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(
            [FromHeader(Name = "userId")] int userId,
            [FromBody] NewCommentDto newCommentDto)
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