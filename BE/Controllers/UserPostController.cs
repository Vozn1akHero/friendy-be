using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;
using BE.Repositories;
using BE.Services.Global;
using BE.Services.Model;
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
        private readonly IRepositoryWrapper _repository;
        private readonly IImageSaver _imageSaver;
        private readonly IUserPostService _userPostService;
        
        public UserPostController(IRepositoryWrapper repository, 
            IImageSaver imageSaverService,
            IUserPostService userPostService)
        {
            _repository = repository;
            _imageSaver = imageSaverService;
            _userPostService = userPostService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRangeByUserId([FromQuery(Name = "userId")] int userId,
            [FromQuery(Name = "startIndex")] int startIndex,
            [FromQuery(Name = "length")] int length)
        {
            var posts = await _userPostService
                .GetRangeByUserIdAsync(userId, startIndex, length);
            return Ok(posts);
        }
        
        [HttpGet("with-min-date")]
        [Authorize]
        public async Task<IActionResult> GetRangeByDate([FromQuery(Name = "userId")] int userId,
            [FromQuery(Name = "minDate")] DateTime date,
            [FromQuery(Name = "length")] int length)
        {
            var posts = await _userPostService
                .GetRangeByMinDateAsync(userId, date, length);
            return Ok(posts);
        }
        
        [HttpGet("last")]
        [Authorize]
        public async Task<IActionResult> GetLastByUserId([FromQuery(Name = "userId")] int userId,
            [FromQuery(Name = "length")] int length)
        {
            var posts = await _userPostService
                .GetLastByUserIdAsync(userId, length);
            return Ok(posts);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserPost([FromForm(Name = "image")] IFormFile image,
            [FromForm(Name = "content")] string content,
            [FromHeader(Name = "userId")] int userId)
        {
            string imagePath = await _imageSaver
                .SaveAndReturnImagePath(image, "EventPost", userId);
            var newPost = new Post
            {
                Content = content, ImagePath = imagePath, Date = DateTime.Now
            };
            await _repository.Post.CreateAsync(newPost);
            var newUserPost = await _userPostService.CreateAndReturnAsync(new UserPost
            {
                UserId = userId, PostId = newPost.Id
            });
            return Ok(newUserPost);
        }

/*
        [HttpGet]
        [Authorize]
        [Route("all/{id}")]
        public async Task<IActionResult> GetAllById(int id)
        {
            var posts = await _repository.UserPost.GetByIdAsync(id);
            return Ok(posts);
        }
        */

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id, [FromHeader(Name = "userId")] int userId)
        {
            var post = await _userPostService.GetByPostId(id, userId);
            return Ok(post);
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
        
        
    }
}