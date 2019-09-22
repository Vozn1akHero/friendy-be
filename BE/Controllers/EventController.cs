using System.Threading.Tasks;
using BE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event")]
    public class EventController : Controller
    {
        private IRepositoryWrapper _repositoryWrapper;

        public EventController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpGet]
        [Authorize]
        [Route("getExampleEvents")]
        public async Task<IActionResult> GetExampleEvents()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("getLoggedInUserEvents")]
        public async Task<IActionResult> GetLoggedInUserEvents([FromHeader(Name = "userId")] int userId)
        {
            var events = await _repositoryWrapper.UserEvents.GetEventsByUserId(userId);
            
            return Ok(events);
        }

        [HttpGet]
        [Authorize]
        [Route("getLoggedInUserAdministeredEvents")]
        public async Task<IActionResult> GetLoggedInUserAdministeredEvents([FromHeader(Name = "userId")] int userId)
        {
            var events = await _repositoryWrapper.EventAdmins.GetUserAdministeredEvents(userId);
            return Ok(events);
        }
        
    }
}