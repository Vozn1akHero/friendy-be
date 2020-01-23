using System.Linq;
using System.Threading.Tasks;
using BE.CustomAttributes;
using BE.Interfaces;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-participant")]
    public class EventParticipantController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private readonly IEventParticipantService _eventParticipantService;

        public EventParticipantController(IRepositoryWrapper repository, 
            IEventParticipantService eventParticipantService)
        {
            _repository = repository;
            _eventParticipantService = eventParticipantService;
        }

        [HttpGet("exemplary/{eventId}")]
        public async Task<IActionResult> GetExemplary(int eventId)
        {
            return Ok(await _repository
                .EventParticipants.GetExemplaryAsync(eventId));
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetRange([FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length, 
            [FromQuery(Name = "eventId")] int eventId)
        {
            return Ok(await _repository.EventParticipants.GetRangeAsync(eventId, startIndex, length));
        }
        
        [HttpGet("range-detailed")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> GetRangeDetailed([FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length, 
            [FromQuery(Name = "eventId")] int eventId)
        {
            var filteredList = await _repository.EventParticipants.GetRangeDetailedAsync(eventId, startIndex, length);
            return Ok(filteredList);
        }

        [HttpGet("filter/{keyword}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> FilterByKeyword(string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var filteredList = await _repository.EventParticipants.FilterByKeywordAsync(keyword);
            //var filteredListWoAdmin = filteredList.Where(e => e.Id != userId);
            return Ok(filteredList);
        }

        [HttpGet("{eventId}/current-user-status")]
        [Authorize]
        public async Task<IActionResult> IsLoggedInUserEventParticipant(int eventId, [FromHeader(Name = "userId")] int userId)
        {
            var res = await _repository.EventParticipants.IsEventParticipant(userId, eventId);
            return Ok(res);
        }

        [HttpPost("{id}/ban/{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public IActionResult BanParticipant(int id,
            int eventId,
            [FromHeader(Name = "userId")] int userId)
        {
            if (id == userId) return UnprocessableEntity();
            _eventParticipantService.BanParticipant(id, eventId);
            return Ok();
        }

        [HttpPost("{id}/unban/{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public IActionResult UnbanParticipant(int id,
            int eventId,
            [FromHeader(Name = "userId")] int userId)
        {
            if (id == userId) return UnprocessableEntity();
            _eventParticipantService.UnbanParticipant(id, eventId);
            return Ok();
        }

        [HttpDelete("{id}/leave/{eventId}")]
        [Authorize]
        [AuthorizeEventParticipant]
        public IActionResult Leave(int id, int eventId)
        {
            _repository.EventParticipants.DeleteByUserIdAndEventId(id, eventId);
            return Ok();
        }

        [HttpGet("{eventId}/banned")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> GetListOfBannedUsersByEventId(int eventId)
        {
            var bannedUsers = await _eventParticipantService
                .FindBannedUsersByEventIdAsync(eventId);
            return Ok(bannedUsers);
        }
        
        [HttpDelete("{id}/remove/{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> RemoveParticipant(int id,
            int eventId,
            [FromHeader(Name = "userId")] int userId)
        {
            if (id == userId)
            {
                return UnprocessableEntity();
            }

            return Ok();
        }
    }
}