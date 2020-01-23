using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Helpers;
using BE.Interfaces;
using BE.Models;
using BE.Services;
using BE.Services.Elasticsearch;
using BE.Services.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BE.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private IRepositoryWrapper _repository;
        private IJwtConf _jwtConf;
        private IUserDataIndexing _userDataIndexing;
        
        public AuthController(IAuthenticationService authenticationService, 
        IRepositoryWrapper repository, 
        IJwtConf jwtConf,
        IUserDataIndexing userDataIndexing)
        {
            _authenticationService = authenticationService;
            _repository = repository;
            _jwtConf = jwtConf;
            _userDataIndexing = userDataIndexing;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateNewUser([FromBody] 
            NewUserDto user)
        {
            bool emailAvailability = await _authenticationService
                .CheckIfEmailIsAvailable(user.Email);
            if (!emailAvailability)
            {
                return Conflict("Email is already taken");
            }
            //var createdUser = await _authenticationService.CreateAndReturnAsync(user);
            return Ok();
        }
        
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthDataDto
            authData)
        {
/*            bool authenticationRes = await _authenticationService
                .Authenticate(authData.Email, authData.Password);
            if (!authenticationRes)
                return Forbid();*/
            var user = await _repository.User.GetByEmailAsync(authData.Email);
            var claims = new List<Claim>
            {
                new Claim("sub", user.Name + user.Surname),
                new Claim("id", user.Id.ToString()),
                new Claim("email", authData.Email)
            };
            string token = _jwtConf.GenerateJwt(claims);
            HttpContext.Response.Cookies.Append(
                "SESSION_TOKEN",
                "Bearer " + token,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = false
                });
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("logout")]
        public IActionResult LogOut([FromHeader(Name = "Authorization")] string token)
        {
            if (token != null)
            {
                HttpContext.Response.Cookies.Delete("SESSION_TOKEN");
                return Ok();
            } 
            return UnprocessableEntity();
        }
        
        [HttpGet]
        [Route("status")]
        public IActionResult GetUserAuthStatus([FromHeader(Name = "Authorization")] string token)
        {
            if (token != null)
            {
                string cutToken = token.Split(" ")[1];
                bool tokenValidity = _jwtConf.ValidateJwt(cutToken);
                if (!tokenValidity)
                {
                    HttpContext.Response.Cookies.Delete("SESSION_TOKEN");
                    return Unauthorized();
                }
                return Ok();
            }
            return Unauthorized();
        }
    }
}