﻿using System;
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
        private IAvatarConverterService _userAvatarConverterService;
        public UserController(IRepositoryWrapper repository,
            IAvatarConverterService userAvatarConverterService)
        {
            _repository = repository;
            _userAvatarConverterService = userAvatarConverterService;
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
            var user = await _repository.User.GetUserByIdAsync(id);
            if(user != null) return Ok(user);
            return NotFound();
        }
        
        
        [HttpGet]
        [Authorize]
        [Route("getUser")]
        public async Task<IActionResult> GetUser()
        {
            string sessionToken = HttpContext.Request.Cookies["SESSION_TOKEN"];
            var user = await _repository.User.GetUserAsync(sessionToken);
            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}/avatar")]
        public async Task<IActionResult> GetUserAvatar([FromHeader(Name = "userId")] int userId)
        {
            var avatar = await _repository.User.GetAvatarByIdAsync(userId);
            return Ok(avatar);
        }

        
        [HttpGet]
        [Authorize]
        [Route("{userId}/background")]
        public async Task<IActionResult> GetProfileBackground([FromHeader(Name = "userId")] int userId)
        {
            var background = await _repository.User.GetProfileBackgroundByIdAsync(userId);
            return Ok(background);
        }
        
        [HttpGet("profile-belonging/{id}")]
        [Authorize]
        public IActionResult GetProfileBelonging(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            if (id == userId) return Ok();
            return Conflict();
        }

        [HttpGet("profile-id")]
        [Authorize]
        public IActionResult GetProfileId([FromHeader(Name = "userId")] int userId)
        {
            return Ok(userId);
        }
        
        [HttpPut]
        [Authorize]
        [Route("avatar")]
        public async Task<IActionResult> UpdateUserAvatar([FromForm(Name = "newAvatar")] IFormFile newAvatar,
            [FromHeader(Name = "userId")] int userId)
        {
            int rand = new Random().Next();
            string fileName = Convert.ToString(userId) + "_" + rand + newAvatar.FileName;
           // string path = "wwwroot/UserPost/" + fileName;
            var path = Path.Combine("wwwroot/UserAvatar", fileName);
            using (var stream = new FileStream(path, FileMode.OpenOrCreate))  
            {  
                await newAvatar.CopyToAsync(stream);
            }
            await _repository.User.UpdateAvatarAsync(path, userId);
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
    }
}