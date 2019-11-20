using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Diagnostics.Telemetry;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/user-post")]
    public class UserPostController : Controller
    {
        private readonly IHubContext<PostHub> _hubContext;
        private readonly IRepositoryWrapper _repository;
        private readonly IImageProcessingService _imageProcessingService;

        public UserPostController(IRepositoryWrapper repository, 
            IHubContext<PostHub> hubContext,
            IImageProcessingService imageProcessingService)
        {
            _repository = repository;
            _hubContext = hubContext;
            _imageProcessingService = imageProcessingService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserPost([FromForm(Name = "image")] IFormFile image,
            [FromForm(Name = "content")] string content,
            [FromHeader(Name = "userId")] int userId)
        {
            string imagePath = await _imageProcessingService
                .SaveAndReturnImagePath(image, "EventPost", userId);
            var newPost = new Post
            {
                Content = content, ImagePath = imagePath, Date = DateTime.Now
            };
            await _repository.Post.CreateAsync(newPost);
            var newUserPost = new UserPost
            {
               UserId = userId, PostId = newPost.Id
            };
            await _repository.UserPost.CreateAsync(newUserPost);
            return Ok(newPost);
        }

        [HttpGet]
        [Authorize]
        [Route("all/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var posts = await _repository.UserPost.GetByIdAsync(id);
            return Ok(posts);
        }
        
/*        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> RemoveUserPostById([FromRoute] int id)
        {
/*            await _repository.PostLike.RemoveLikesByPostIdAsync(id);
            await _repository.Comment.RemovePostCommentsByPostIdAsync(id);
            await _repository.UserPost.RemoveByIdAsync(id);#1#
            await _repository.Post.RemoveByIdAsync(id);
            return Ok();
        }*/
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPostsByUserId([FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length, [FromQuery(Name = "userId")] int userId)
        {
            var entries = await _repository.UserPost.GetRangeByIdAsync(userId ,startIndex, length);
            return Ok(entries);
        }
        
        [HttpGet]
        [Authorize]
        [Route("current")]
        public async Task<IActionResult> GetLoggedInUserPosts([FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length,
            [FromHeader(Name = "userId")] int userId)
        {
            var entries = await _repository.UserPost.GetRangeByIdAsync(userId ,startIndex, length);
            return Ok(entries);
        }
    }
}