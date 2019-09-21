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
        private readonly IJwtService _jwtService;
        
        public PostController(IRepositoryWrapper repositoryWrapper, IHubContext<PostHub> hubContext, IJwtService jwtService)
        {
            _repositoryWrapper = repositoryWrapper;
            _hubContext = hubContext;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Authorize]
        [Route("createUserPost")]
        public async Task<IActionResult> CreateUserPost([FromBody] UserPost post)
        {
            if (post.Image != null && post.Image.Length >= 8000)
            {
                return BadRequest("IMAGE IS TOO HEAVY");
            }
            var newPost = new UserPost();
            newPost.UserId = post.UserId;
            newPost.Content = post.Content;
            newPost.Image = post.Image;
            newPost.Date = DateTime.Now;
            await _repositoryWrapper.UserPost.CreateUserPost(newPost);
            return Ok(newPost);
        }
        
        
        [HttpGet]
        [Authorize]
        [Route("getLoggedInUserPostsByToken")]
        public async Task<IActionResult> GetLoggedInUserPostsByToken()
        {
            string sessionToken = HttpContext.Request.Cookies["SESSION_TOKEN"];
            var user = await _repositoryWrapper.User.GetUser(sessionToken);
            var entries = await _repositoryWrapper.UserPost.GetLoggedInUserPostsById(user.Id);
            return Ok(entries);
        }
        
                
        [HttpDelete]
        [Authorize]
        [Route("removeUserPostById/{id}")]
        public async Task<IActionResult> RemoveUserPostById([FromRoute] int id)
        {
            /*string sessionToken = HttpContext.Request.Cookies["SESSION_TOKEN"];
            var user = await _repositoryWrapper.User.GetUser(sessionToken);*/
            await _repositoryWrapper.UserPost.RemovePostById(id);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("likeUserPostById/{id}")]
        public async Task<IActionResult> LikeUserPostById([FromRoute] int id)
        {
            int userId = _jwtService.GetUserIdFromJwt(HttpContext.Request.Cookies["SESSION_TOKEN"]);
            var newLike = new UserPostLikes
            {
                UserId = userId,
                UserPostId = id
            };
            await _repositoryWrapper.UserPostLikes.CreatePostLike(newLike);
            return Ok(newLike);
        }

        [HttpGet]
        [Authorize]
        [Route("getLoggedInUserPosts")]
        public async Task<IActionResult> GetLoggedInUserPosts()
        {
            string token = HttpContext.Request.Cookies["SESSION_TOKEN"];
            int userId = _jwtService.GetUserIdFromJwt(token);
            var entries = await _repositoryWrapper.UserPost.GetLoggedInUserPostsById(userId);
            return Ok(entries);
        }
    }
}