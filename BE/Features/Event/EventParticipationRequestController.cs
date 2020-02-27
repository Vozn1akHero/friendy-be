using System.Threading.Tasks;
using BE.Features.Event.Dtos;
using BE.Features.Event.Services;
using BE.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Event
{
    [ApiController]
    [Route("api/event-participation-request")]
    public class EventParticipationRequestController : ControllerBase
    {
        private readonly IEventParticipationRequestService
            _eventParticipationRequestService;

        public EventParticipationRequestController(
            IEventParticipationRequestService eventParticipationRequestService)
        {
            _eventParticipationRequestService = eventParticipationRequestService;
        }

        [HttpPost("{eventId}")]
        [Authorize]
        public async Task<IActionResult> Create(int eventId,
            [FromHeader(Name = "userId")] int issuerId)
        {
            var newRequest = await _eventParticipationRequestService
                .CreateAndReturnAsync(issuerId, eventId);
            return Created($"api/event-participation-request/{eventId}", newRequest);
        }

        [HttpDelete("{eventId}/{issuerId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> DeleteWithAdminRights(int userId, int eventId)
        {
            await _eventParticipationRequestService.DeleteAsync(userId, eventId);
            return Ok();
        }

        [HttpDelete("{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> Delete(int eventId,
            [FromHeader(Name = "userId")] int issuerId)
        {
            await _eventParticipationRequestService.DeleteAsync(issuerId, eventId);
            return Ok();
        }

        [HttpGet("filter/{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public IActionResult FindByKeyword(int eventId,
            [FromQuery(Name = "q")] string keyword)
        {
            var list = _eventParticipationRequestService.FindByKeyword(eventId, keyword);
            return Ok(list);
        }
        
        [HttpGet("{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public IActionResult GetWithPagination(int eventId,
            [FromQuery(Name = "page")] int page)
        {
            var list = _eventParticipationRequestService.GetWithPagination(eventId, page);
            return Ok(list);
        }
        
        [HttpDelete("{eventId}/{userId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> DeleteSpecific(int eventId,
            int userId)
        {
            await _eventParticipationRequestService.DeleteByIssuerId(userId, eventId);
            return Ok();
        }
        
        [HttpPost("{eventId}/{confirm}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> ConfirmSpecific([FromBody] RequestConfirmationDto confirmationDto)
        {
            await _eventParticipationRequestService.ConfirmByIssuerId(confirmationDto.IssuerId,
                confirmationDto.EventId);
            return Ok();
        }
    }
}