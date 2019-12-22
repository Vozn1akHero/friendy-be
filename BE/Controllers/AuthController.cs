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
using BE.Services.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        private IRepositoryWrapper _repository;
        private IJwtService _jwtService;

        public AuthController(IAuthenticationService authenticationService, 
            IRepositoryWrapper repository, IJwtService jwtService)
        {
            _authenticationService = authenticationService;
            _repository = repository;
            _jwtService = jwtService;
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
            await _authenticationService.Create(user);
            return Ok();
        }
        
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthDataDto authData)
        {
            bool authenticationRes = await _authenticationService.Authenticate(authData.Email, authData.Password);
            if (!authenticationRes)
                return Forbid();
            var user = await _repository.User.GetByEmailAsync(authData.Email);
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", authData.Email)
            };
            string token = _jwtService.GenerateJwt(claims);
            HttpContext.Response.Cookies.Append(
                "SESSION_TOKEN",
                "Bearer " + token,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = false
                });
            var session = await _repository.AuthenticationSession.CreateAndReturn(token);
            await _authenticationService.SetSessionIdByUserId(session.Id, user.Id);
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> LogOut([FromHeader(Name = "Authorization")] string token)
        {
            string cutToken = token.Split(" ")[1];
            await _authenticationService.LogOut(cutToken);
            HttpContext.Response.Cookies.Delete("SESSION_TOKEN");
            return Ok();
        }
        
        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> GetUserAuthStatus([FromHeader(Name = "Authorization")] string token)
        {
            if (token != null)
            {
                string cutToken = token.Split(" ")[1];
                bool tokenValidity = _jwtService.ValidateJwt(cutToken);
                if (!tokenValidity)
                {
                    await _authenticationService.LogOut(cutToken);
                    HttpContext.Response.Cookies.Delete("SESSION_TOKEN");
                    return Unauthorized();
                }
                return Ok();
            }
            return Unauthorized();
        }
    }
}