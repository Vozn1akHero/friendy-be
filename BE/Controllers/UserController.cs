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
        
        public UserController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _repository.User.GetAllUsersAsync();
            return Ok(users);
        }
        
        [HttpGet]
        [Authorize]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.User.GetUserById(id);
            if(user != null) return Ok(user);
            return NotFound();
        }
        
        
        [HttpGet]
        [Authorize]
        [Route("getUser")]
        public async Task<IActionResult> GetUser()
        {
            string sessionToken = HttpContext.Request.Cookies["SESSION_TOKEN"];
            var user = await _repository.User.GetUser(sessionToken);
            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        [Route("getAvatar")]
        public async Task<IActionResult> GetUserAvatar([FromHeader(Name = "userId")] int userId)
        {
            var user = await _repository.User.GetUserById(userId);
            var content = new FileStream(user.Avatar, FileMode.Open, FileAccess.Read, FileShare.Read);
            var response = File(content, "application/octet-stream");//FileStreamResult
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        [Route("updateAvatar")]
        public async Task<IActionResult> UpdateUserAvatar(IFormFile newAvatar, [FromHeader(Name = "userId")] int userId)
        {
            string newPath = newAvatar.Name + userId;
            
            var path = Path.Combine("wwwroot/UserAvatar", newPath);  
  
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))  
            {  
                await newAvatar.CopyToAsync(stream);
            }

            await _repository.User.UpdateAvatar(newPath, userId);

            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("getUsersByCriteria")]
        public async Task<IActionResult> GetUsersByCriteria(
            [FromBody] UsersLookUpCriteriaDto usersLookUpCriteriaDto)
        {
            var users = await _repository.User.GetUsersByCriteria(usersLookUpCriteriaDto);
            return Ok(users);
        }
    }
}