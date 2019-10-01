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
    [Route("api/post")]
    public class PostController : Controller
    {
        private readonly IHubContext<PostHub> _hubContext;
        private readonly IRepositoryWrapper _repositoryWrapper;
       // private readonly IJwtService _jwtService;
        
        public PostController(IRepositoryWrapper repositoryWrapper, IHubContext<PostHub> hubContext)
        {
            _repositoryWrapper = repositoryWrapper;
            _hubContext = hubContext;
            //_jwtService = jwtService;
        }

        [HttpPost]
        [Authorize]
        [Route("createUserPost")]
        public async Task<IActionResult> CreateUserPost([FromBody] UserPost post,
            [FromHeader(Name = "userId")] int userId)
        {
            if (post.Image != null && post.Image.Length >= 8000)
            {
                return BadRequest("IMAGE IS TOO HEAVY");
            }
            var newPost = new UserPost
            {
                UserId = userId, Content = post.Content, Image = post.Image, Date = DateTime.Now
            };
            await _repositoryWrapper.UserPost.CreateUserPost(newPost);
            return Ok(newPost);
        }
        
        
        [HttpGet]
        [Authorize]
        [Route("getLoggedInUserPostsByToken")]
        public async Task<IActionResult> GetLoggedInUserPostsByToken([FromHeader(Name = "userId")] int userId)
        {
            var entries = await _repositoryWrapper.UserPost.GetById(userId);
            return Ok(entries);
        }

        [HttpGet]
        [Authorize]
        [Route("getById")]
        public async Task<IActionResult> GetById(int id)
        {
            var posts = await _repositoryWrapper.UserPost.GetById(id);
            return Ok(posts);
        }
        
        [HttpDelete]
        [Authorize]
        [Route("removeUserPostById/{id}")]
        public async Task<IActionResult> RemoveUserPostById([FromRoute] int id)
        {
            await _repositoryWrapper.UserPostLikes.RemovePostLikes(id);
            await _repositoryWrapper.UserPostComments.RemovePostComments(id);
            await _repositoryWrapper.UserPost.RemovePostById(id);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("likeUserPostById/{id}")]
        public async Task<IActionResult> LikeUserPostById([FromRoute] int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var newLike = new UserPostLikes
            {
                UserId = userId,
                UserPostId = id
            };
            await _repositoryWrapper.UserPostLikes.CreatePostLike(newLike);
            return Ok(newLike);
        }

        [HttpPut]
        [Authorize]
        [Route("unlikeUserPostById/{id}")]
        public async Task<IActionResult> UnlikePostById([FromRoute] int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var curLike = new UserPostLikes
            {
                UserId = userId,
                UserPostId = id
            };
            await _repositoryWrapper.UserPostLikes.RemovePostLike(curLike);
            return Ok(curLike);
        }
        
        [HttpGet]
        [Authorize]
        [Route("getLoggedInUserPosts")]
        public async Task<IActionResult> GetLoggedInUserPosts([FromHeader(Name = "userId")] int userId)
        {
            var entries = await _repositoryWrapper.UserPost.GetById(userId);
            return Ok(entries);
        }
    }
}