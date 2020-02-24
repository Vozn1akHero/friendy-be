using System;
using System.IO;
using System.Threading.Tasks;
using BE.ElasticSearch;
using BE.Features.User.Services;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.User
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IUserDataIndexing _userDataIndexing;
        private readonly IUserDataService _userDataService;

        public UserController(IRepositoryWrapper repository,
            IUserDataIndexing userDataIndexing, IUserDataService userDataService)
        {
            _repository = repository;
            _userDataIndexing = userDataIndexing;
            _userDataService = userDataService;
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<Models.User> GetById(int id)
        {
            var user = await _userDataService.GetByIdAsync(id);
            return user;
        }

        [HttpGet("{id}/with-selected-fields")]
        [Authorize]
        public async Task<IActionResult> GetByIdWithSelectedFields(int id,
            [FromQuery(Name = "selectedFields")] string selectedFields)
        {
            var selectedFieldsArr = selectedFields.Split(",");
            var fields =
                await _repository.User.GetWithSelectedFields(id, selectedFieldsArr);
            return Ok(fields);
        }

        [HttpGet]
        [Authorize]
        [Route("logged-in/with-selected-fields")]
        public async Task<IActionResult> GetUser([FromHeader(Name = "userId")] int userId,
            [FromQuery(Name = "selectedFields")] string selectedFields)
        {
            var selectedFieldsArr = selectedFields.Split(",");
            var user =
                await _repository.User.GetWithSelectedFields(userId, selectedFieldsArr);
            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        [Route("logged-in/extended")]
        public async Task<IActionResult> GetLoggedInExtendedInfo(
            [FromHeader(Name = "userId")] int userId)
        {
            var user = await _userDataService.GetExtendedById(userId);
            return Ok(user);
        }

        [HttpGet]
        [Route("{userId}/extended")]
        public async Task<IActionResult> GetExtendedInfo(int userId)
        {
            var user = await _userDataService.GetExtendedById(userId);
            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        [Route("logged-in")]
        public async Task<IActionResult> GetLoggedInUserWithSelectedFields(
            [FromHeader(Name = "userId")] int userId)
        {
            var user = await _repository.User.GetByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}/avatar")]
        public async Task<IActionResult> GetUserAvatar(int userId)
        {
            var avatar = await _repository.User.GetAvatarByIdAsync(userId);
            return Ok(avatar);
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}/background")]
        public async Task<IActionResult> GetProfileBackground(int userId)
        {
            var background = await _repository.User.GetProfileBackgroundByIdAsync(userId);
            return Ok(background);
        }

        [HttpGet("profile-belonging/{id}")]
        [Authorize]
        public IActionResult GetProfileBelonging(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            return Ok(id == userId);
        }


        [HttpPut]
        [Authorize]
        [Route("avatar")]
        public async Task<IActionResult> UpdateUserAvatar(
            [FromForm(Name = "newAvatar")] IFormFile newAvatar,
            [FromHeader(Name = "userId")] int userId)
        {
            var rand = new Random().Next();
            var fileName = Convert.ToString(userId) + "_" + rand + newAvatar.FileName;
            var path = Path.Combine("wwwroot/UserAvatar", fileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                await newAvatar.CopyToAsync(stream);
            }

            await _repository.User.UpdateAvatarAsync(path, userId);
            await _userDataIndexing.UpdateAvatar(userId, path);
            return Ok(path);
        }

        [HttpPut]
        [Authorize]
        [Route("background")]
        public async Task<IActionResult> UpdateProfileBackground(
            [FromForm(Name = "newBackground")] IFormFile newBackground,
            [FromHeader(Name = "userId")] int userId)
        {
            var rand = new Random().Next();
            var fileName = Convert.ToString(userId) + "_" + rand + newBackground.FileName;
            var path = Path.Combine("wwwroot/ProfileBackground", fileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                await newBackground.CopyToAsync(stream);
            }

            await _repository.User.UpdateProfileBackgroundAsync(path, userId);
            return Ok(path);
        }

        [HttpGet("index")]
        public async Task<IActionResult> GetDataForEsCluster()
        {
            var users = await _userDataService.GetDataForElasticsearchIndex();
            await _userDataIndexing.Bulk(users);
            return Ok(users);
        }

        [HttpGet("interests/find/{title}")]
        public async Task<IActionResult> FindInterestsByTitle(string title)
        {
            var interests = await _userDataService.FindInterestsByTitle(title);
            if (interests == null) return NotFound();
            return Ok(interests);
        }
    }
}