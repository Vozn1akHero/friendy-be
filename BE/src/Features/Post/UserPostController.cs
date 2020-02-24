using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.PostDtos;
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

        [HttpGet("range")]
        [Authorize]
        public ActionResult
            GetRange(int userId, int startIndex, int length)
        {
            var posts =
                _userPostService.GetRangeByUserId(userId, startIndex, length);
            if (posts == null) return NotFound();
            return Ok(posts);
        }

        [HttpGet("range")]
        [Authorize]
        public ActionResult<IEnumerable<UserPostDto>> GetByPage(
            [FromQuery(Name = "userId")] int userId,
            [FromQuery(Name = "page")] int page)
        {
            var posts = _userPostService
                .GetByPage(userId, page);
            return Ok(posts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync(
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