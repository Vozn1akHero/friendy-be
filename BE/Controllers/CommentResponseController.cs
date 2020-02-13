using System.Threading.Tasks;
using BE.Interfaces;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/comment-response")]
    public class CommentResponseController : ControllerBase
    {
        private ICommentResponseService _commentResponseService;

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
            var createdEntity = await _commentResponseService.Like(id, userId);
            return CreatedAtAction("Like", createdEntity);
        }
        
        [HttpDelete("unlike/{id}")]
        [Authorize]
        public async Task<IActionResult> Unlike(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            await _commentResponseService.Unlike(id, userId);
            return Ok();
        }

        [HttpGet("all/{commentId}")]
        [Authorize]
        public async Task<IActionResult> GetAllResponsesByCommentIdAsync(int commentId,
            [FromHeader(Name = "userId")] int userId)
        {
            var responses =
                await _commentResponseService.GetAllCommentResponsesAsync(commentId, userId);
            return Ok(responses);
        }
    }
}