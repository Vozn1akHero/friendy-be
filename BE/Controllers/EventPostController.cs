using System;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-post")]
    public class EventPostController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public EventPostController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        
        /*[HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserPost([FromBody] EventPost post,
            [FromHeader(Name = "userId")] int userId)
        {
            if (post.Image != null && post.Image.Length >= 8000)
            {
                return BadRequest("IMAGE IS TOO BIG");
            }
            var newPost = new EventPost
            {
                EventId = post.EventId,
                Content = post.Content,
                Image = post.Image, 
                Date = DateTime.Now
            };
            //await _repository.UserPost.CreateUserPost(newPost);
            return Ok(newPost);
        }*/
    }
}