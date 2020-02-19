using System;
using System.Threading.Tasks;
using BE.Features.Post.Services;
using BE.Helpers;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.User
{
    [ApiController]
    [Route("api/user-post")]
    public class UserPostController : Controller
    {
        private readonly IUserPostService _userPostService;

        public UserPostController(IUserPostService userPostService)
        {
            _userPostService = userPostService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRangeByUserId(
            [FromQuery(Name = "userId")] int userId,
            [FromQuery(Name = "startIndex")] int startIndex,
            [FromQuery(Name = "length")] int length)
        {
            var posts = await _userPostService
                .GetRangeByUserIdAsync(userId, startIndex, length);
            return Ok(posts);
        }

        [HttpGet("with-min-date")]
        [Authorize]
        public async Task<IActionResult> GetRangeByDate(
            [FromQuery(Name = "userId")] int userId,
            [FromQuery(Name = "minDate")] DateTime date,
            [FromQuery(Name = "length")] int length)
        {
            var posts = await _userPostService
                .GetRangeByMinDateAsync(userId, date, length);
            return Ok(posts);
        }

        [HttpGet("last")]
        [Authorize]
        public async Task<IActionResult> GetLastByUserId(
            [FromQuery(Name = "userId")] int userId,
            [FromQuery(Name = "length")] int length)
        {
            var posts = await _userPostService
                .GetLastByUserIdAsync(userId, length);
            return Ok(posts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserPost(
            [FromForm(Name = "image")] IFormFile image,
            [FromForm(Name = "content")] string content,
            [FromHeader(Name = "userId")] int userId)
        {
            var newUserPost =
                await _userPostService.CreateAndReturnAsync(userId, content, image);
            return Ok(newUserPost);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public IActionResult GetById(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var post = _userPostService.GetByPostId(id, userId);
            return Ok(post);
        }
    }
}