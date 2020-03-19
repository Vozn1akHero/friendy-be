using System.Threading.Tasks;
using BE.Features.Comment.Dtos;
using BE.Features.Comment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Comment
{
    [ApiController]
    [Route("api/comment-response")]
    public class CommentResponseController : ControllerBase
    {
        private readonly ICommentResponseService _commentResponseService;

        public CommentResponseController(ICommentResponseService commentResponseService)
        {
            _commentResponseService = commentResponseService;
        }

        [HttpPost("like/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Like(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var createdEntity = await _commentResponseService.LikeAsync(id, userId);
            return CreatedAtAction("Like", createdEntity);
        }

        [HttpDelete("unlike/{id}")]
        [Authorize]
        public async Task<IActionResult> Unlike(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            await _commentResponseService.UnlikeAsync(id, userId);
            return Ok();
        }

        [HttpGet("all/{commentId}")]
        [Authorize]
        public async Task<IActionResult> GetAllResponsesByCommentIdAsync(int commentId,
            [FromHeader(Name = "userId")] int userId)
        {
            var responses =
                await _commentResponseService.GetAllCommentResponsesAsync(commentId,
                    userId);
            return Ok(responses);
        }

        [HttpPost("to-response")]
        [Authorize]
        public async Task<IActionResult> CreateResponseToResponse(
            [FromBody] TransferredResponseToResponseDto responseDto,
            [FromForm] IFormFile file,
            [FromHeader(Name = "userId")] int userId)
        {
            var createdEntry = await _commentResponseService.CreateAndReturnDtoAsync
                (responseDto, userId, file);
            return CreatedAtAction("CreateResponseToResponse", createdEntry);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(
            [FromBody] NewCommentResponseDto responseDto,
            [FromForm] IFormFile file,
            [FromHeader(Name = "userId")] int userId)
        {
            var createdEntry = await _commentResponseService.CreateAndReturnDtoAsync
                (responseDto, userId, file);
            return CreatedAtAction("Create", createdEntry);
        }
    }
}