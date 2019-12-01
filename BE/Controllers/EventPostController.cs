using System;
using System.IO;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-post")]
    public class EventPostController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IImageProcessingService _imageProcessingService;

        public EventPostController(IRepositoryWrapper repository,
            IImageProcessingService imageProcessingService)
        {
            _repository = repository;
            _imageProcessingService = imageProcessingService;
        }
        
        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> CreateEventPost([FromForm(Name = "image")] IFormFile image,
            [FromForm(Name = "content")] string content,
            [FromQuery(Name = "id")] int eventId,
            [FromHeader(Name = "userId")] int userId)
        {
            string imagePath = await _imageProcessingService
                .SaveAndReturnImagePath(image, "EventPost", userId);
            var newPost = new Post
            {
                Content = content, ImagePath = imagePath, Date = DateTime.Now
            };
            await _repository.Post.CreateAsync(newPost);
            var newUserPost = new EventPost
            {
                EventId = eventId, PostId = newPost.Id
            };
            await _repository.EventPost.CreateAsync(newUserPost);
            return Ok(newPost);
        }
        
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id, 
            [FromHeader(Name = "userId")] int userId)
        {
            var post = await _repository.EventPost.GetByIdAuthedAsync(id, userId);
            return Ok(post);
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRangeByEventId([FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length, 
            [FromQuery(Name = "eventId")] int eventId, 
            [FromHeader(Name = "userId")] int userId)
        {
            var entries = await _repository.EventPost.GetRangeByIdAsync(eventId, startIndex, length, userId);
            return Ok(entries);
        }
        
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> RemoveUserPostById([FromRoute] int id)
        {
            await _repository.Post.RemoveByIdAsync(id);
            return Ok();
        }
    }
}