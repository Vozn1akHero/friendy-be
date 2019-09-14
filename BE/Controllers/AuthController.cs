using System.Net;
using System.Threading.Tasks;
using BE.Helpers;
using BE.Interfaces;
using BE.Models;
using BE.Services;
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

        public AuthController(IAuthenticationService authenticationService, IRepositoryWrapper repository)
        {
            _authenticationService = authenticationService;
            _repository = repository;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateNewUser([FromBody] User user)
        {
            try
            {
                bool emailAvailability = await _authenticationService.CheckIfEmailIsAvailable(user.Email);
                if (!emailAvailability)
                {
                    return HTTPHelpers.TextResult(HttpStatusCode.Conflict, "Email is already taken");
                }

                await _authenticationService.Create(user);
                return Ok();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User user)
        {
            try
            {
                var authenticationRes = await _authenticationService.Authenticate(user.Email, user.Password);

                //create a class which contains those messages
                if (authenticationRes.ErrorMsg == "Data is incorrect")
                    return Forbid();

                HttpContext.Response.Cookies.Append(
                    "SESSION_TOKEN",
                    "Bearer " + authenticationRes.Token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false
                    });

                var session = await _repository.Session.CreateSession(authenticationRes.Token);
                await _repository.User.SetSessionId(authenticationRes.UserId, session.Id);
                
                return Ok(new
                {
                    sessionHash = session.Hash
                });
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            string token = Request.Cookies["SESSION_TOKEN"];
            await _authenticationService.LogOut(token);
            HttpContext.Response.Cookies.Delete("SESSION_TOKEN");
            return Ok();
        }
        
        [HttpGet]
        [Authorize]
        [Route("getUserAuthStatus")]
        public IActionResult GetUserAuthStatus()
        {
            return Ok();
        }
    }
}