using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Diagnostics.Telemetry;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/user-post")]
    public class PostController : Controller
    {
        private readonly IHubContext<PostHub> _hubContext;
        private readonly IRepositoryWrapper _repository;
        
        public PostController(IRepositoryWrapper repository, 
            IHubContext<PostHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserPost([FromBody] UserPost post,
            [FromHeader(Name = "userId")] int userId)
        {
            if (post.Image != null && post.Image.Length >= 8000)
            {
                return BadRequest("IMAGE IS TOO BIG");
            }
            var newPost = new UserPost
            {
                UserId = userId, Content = post.Content, Image = post.Image, Date = DateTime.Now
            };
            await _repository.UserPost.CreateUserPost(newPost);
            return Ok(newPost);
        }
        
/*
        
        [HttpGet]
        [Authorize]
        [Route("getLoggedInUserPostsByToken")]
        public async Task<IActionResult> GetLoggedInUserPostsByToken([FromHeader(Name = "userId")] int userId)
        {
            var entries = await _repository.UserPost.GetById(userId);
            return Ok(entries);
        }*/

        [HttpGet]
        [Authorize]
        [Route("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            var posts = await _repository.UserPost.GetById(id);
            return Ok(posts);
        }
        
        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> RemoveUserPostById([FromRoute] int id)
        {
            await _repository.UserPostLikes.RemovePostLikes(id);
            await _repository.UserPostComments.RemovePostComments(id);
            await _repository.UserPost.RemovePostById(id);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("like/{id}")]
        public async Task<IActionResult> LikeUserPostById([FromRoute] int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var newLike = new UserPostLikes
            {
                UserId = userId,
                UserPostId = id
            };
            await _repository.UserPostLikes.CreatePostLike(newLike);
            return Ok(newLike);
        }

        [HttpPut]
        [Authorize]
        [Route("unlike/{id}")]
        public async Task<IActionResult> UnlikePostById([FromRoute] int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var curLike = new UserPostLikes
            {
                UserId = userId,
                UserPostId = id
            };
            await _repository.UserPostLikes.RemovePostLike(curLike);
            return Ok(curLike);
        }
        
        [HttpGet]
        [Authorize]
        [Route("current")]
        public async Task<IActionResult> GetLoggedInUserPosts([FromHeader(Name = "userId")] int userId)
        {
            var entries = await _repository.UserPost.GetById(userId);
            return Ok(entries);
        }
    }
}