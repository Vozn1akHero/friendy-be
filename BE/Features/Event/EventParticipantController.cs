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
        private readonly IRepositoryWrapper _repository;

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
        public async Task<IActionResult> GetRange(
            [FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length,
            [FromQuery(Name = "eventId")] int eventId)
        {
            return Ok(
                await _repository.EventParticipants.GetRangeAsync(eventId, startIndex,
                    length));
        }

        [HttpGet("range-detailed")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> GetRangeDetailed(
            [FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length,
            [FromQuery(Name = "eventId")] int eventId)
        {
            var filteredList =
                await _repository.EventParticipants.GetRangeDetailedAsync(eventId,
                    startIndex, length);
            return Ok(filteredList);
        }

        [HttpGet("filter/{keyword}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> FilterByKeyword(string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var filteredList =
                await _repository.EventParticipants.FilterByKeywordAsync(keyword);
            //var filteredListWoAdmin = filteredList.Where(e => e.Id != userId);
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