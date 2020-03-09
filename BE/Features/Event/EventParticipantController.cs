using System.Threading.Tasks;
using BE.Features.Event.Services;
using BE.Helpers;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Event
{
    [ApiController]
    [Route("api/event-participant")]
    public class EventParticipantController : ControllerBase
    {
        private readonly IEventParticipantService _eventParticipantService;
        
        public EventParticipantController(
            IEventParticipantService eventParticipantService)
        {
            _eventParticipantService = eventParticipantService;
        }

        [HttpGet("range/{eventId}")]
        public IActionResult GetRange(
            [FromQuery(Name = "page")] int page,
            [FromHeader(Name = "userId")] int userId,
            [FromQuery(Name = "length")] int length,
            int eventId)
        {
            var participants = _eventParticipantService.GetRangeByEventIdWithPagination(eventId, page, length, userId);
            return Ok(participants);
        }

        [HttpGet("filter/{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public IActionResult FilterByKeyword(int eventId,
           [FromQuery(Name = "keyword")] string keyword,
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "length")] int length,
            [FromHeader(Name = "userId")] int userId)
        {
            var filteredList = _eventParticipantService.FilterRangeByKeywordAndEventId(eventId, keyword, page,
                length, userId);
            return Ok(filteredList);
        }

        [HttpGet("{eventId}/current-user-status")]
        [Authorize]
        public IActionResult IsLoggedInUserEventParticipant(int eventId,
            [FromHeader(Name = "userId")] int issuerId)
        {
            var res = _eventParticipantService.Status(eventId, issuerId);
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

        [HttpDelete("leave/{eventId}")]
        [Authorize]
        [AuthorizeEventParticipant]
        public async Task<IActionResult> Leave(int? id, int eventId,
            [FromHeader(Name = "userId")] int userId)
        {
            await _eventParticipantService.LeaveAsync(id ?? userId,
                eventId);
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
        public async Task<IActionResult> RemoveParticipant(int id, int eventId)
        {
            await _eventParticipantService.LeaveAsync(id,
                eventId);
            return Ok();
        }
    }
}