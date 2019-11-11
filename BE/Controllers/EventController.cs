using System.Threading.Tasks;
using BE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event")]
    public class EventController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public EventController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        [Route("user/loggedin")]
        public async Task<IActionResult> GetLoggedInUserEvents([FromHeader(Name = "userId")] int userId)
        {
            var events = await _repository
                .UserEvents
                .GetShortenedEventsByUserId(userId);
            
            return Ok(events);
        }

        [HttpGet]
        [Authorize]
        [Route("user/loggedin/administered")]
        public async Task<IActionResult> GetLoggedInUserAdministeredEvents([FromHeader(Name = "userId")] int userId)
        {
            var events = await _repository
                .EventAdmins
                .GetShortenedAdministeredEventsByUserId(userId);
            return Ok(events);
        }
        
        [HttpGet]
        [Authorize]
        [Route("filter/administered")]
        public async Task<IActionResult> FilterAdministeredEvents([FromQuery(Name = "keyword")] string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var events = await _repository.EventAdmins.FilterAdministeredEvents(userId, keyword);
            
            return Ok(events);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var eventData = await _repository.Event.GetById(id);
            return Ok(eventData);
        }
        
        
        
/*        [HttpGet("avatar/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAvatarById(int id)
        {
            var avatar = await _repository.Event.GetAvatarById(id);
            return Ok(avatar);
        }*/
    }
}