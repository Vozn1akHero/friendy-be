using System;
using System.Threading.Tasks;
using BE.Features.Authentication.Dtos;
using BE.Features.Authentication.Services;
using BE.Features.User.Dtos;
using BE.Helpers.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Authentication
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync([FromBody] NewUserDto user)
        {
            try
            {
                var createdUser = await _authenticationService.CreateAndReturnAsync(user);
                return CreatedAtAction("Create", createdUser);
            }
            catch (EmailIsAlreadyTakenException)
            {
                return Conflict(new EmailIsAlreadyTakenException());
            }
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthDataDto
            authData)
        {
            var res = _authenticationService.Authenticate(authData.Email,
                authData.Password);
            if (!res.Result) return Forbid();
            HttpContext.Response.Cookies.Append(
                "SESSION_TOKEN",
                "Bearer " + res.Token,
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
        public IActionResult GetUserAuthStatus(
            [FromHeader(Name = "Authorization")] string token)
        {
            if (token != null)
            {
                var tokenValidity = _authenticationService.CheckUserAuthStatus(token);
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