using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Helpers;
using BE.Interfaces;
using BE.Models;
using BE.Services;
using BE.Services.Elasticsearch;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BE.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IUserDataIndexing _userDataIndexing;
        private IUserDataService _userDataService;
        
        public UserController(IRepositoryWrapper repository, IUserDataIndexing userDataIndexing, IUserDataService userDataService)
        {
            _repository = repository;
            _userDataIndexing = userDataIndexing;
            _userDataService = userDataService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repository.User.GetAllAsync();
            return Ok(users);
        }
        
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.User.GetByIdAsync(id);
            if(user != null) return Ok(user);
            return NotFound();
        }

        [HttpGet("{id}/with-selected-fields")]
        [Authorize]
        public async Task<IActionResult> GetByIdWithSelectedFields(int id, 
            [FromQuery(Name = "selectedFields")] string selectedFields)
        {
            string[] selectedFieldsArr = selectedFields.Split(",");
            var fields = await _repository.User.GetWithSelectedFields(id, selectedFieldsArr);
            return Ok(fields);
        }
        
        [HttpGet]
        [Authorize]
        [Route("logged-in/with-selected-fields")]
        public async Task<IActionResult> GetUser([FromHeader(Name = "userId")] int userId,
            [FromQuery(Name = "selectedFields")] string selectedFields)
        {
            string[] selectedFieldsArr = selectedFields.Split(",");
            var user = await _repository.User.GetWithSelectedFields(userId, selectedFieldsArr);
            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        [Route("logged-in/extended")]
        public async Task<IActionResult> GetLoggedInExtendedInfo([FromHeader(Name = "userId")] int userId)
        {
            //var user = await _repository.User.GetExtendedInfoById(userId);
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
        public async Task<IActionResult> GetLoggedInUserWithSelectedFields([FromHeader(Name = "userId")] int userId)
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

/*        [HttpGet("profile-id")]
        [Authorize]
        public IActionResult GetProfileId([FromHeader(Name = "userId")] int userId)
        {
            return Ok(userId);
        }*/
        
        [HttpPut]
        [Authorize]
        [Route("avatar")]
        public async Task<IActionResult> UpdateUserAvatar([FromForm(Name = "newAvatar")] IFormFile newAvatar,
            [FromHeader(Name = "userId")] int userId)
        {
            int rand = new Random().Next();
            string fileName = Convert.ToString(userId) + "_" + rand + newAvatar.FileName;
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
        public async Task<IActionResult> UpdateProfileBackground([FromForm(Name = "newBackground")] IFormFile newBackground,
            [FromHeader(Name = "userId")] int userId)
        {
            int rand = new Random().Next();
            string fileName = Convert.ToString(userId) + "_" + rand + newBackground.FileName;
            var path = Path.Combine("wwwroot/ProfileBackground", fileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))  
            {  
                await newBackground.CopyToAsync(stream); 
            }
            await _repository.User.UpdateProfileBackgroundAsync(path, userId);
            return Ok(path);
        }

        [HttpGet("is-online/{id}")]
        [Authorize]
        public async Task<IActionResult> IsUserOnlineAsync(int id)
        {
            var session = await _repository.Session.GetByUserId(id);
            var status = session.ConnectionEnd == null;
            return Ok(new
            {
                Status = status,
                session.ConnectionStart,
                session.ConnectionEnd
            });
        }
    }
}