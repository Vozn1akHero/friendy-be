using System.Threading.Tasks;
using BE.Features.Event.Dtos;
using BE.Features.Event.Services;
using BE.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Event
{
    [ApiController]
    [Route("api/event-data")]
    public class EventDataController : ControllerBase
    {
        private readonly IEventDataService _eventDataService;

        public EventDataController(IEventDataService eventDataService)
        {
            _eventDataService = eventDataService;
        }

        [HttpPatch("{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> Update(int eventId,
            [FromBody] EventDataDto eventDataDto)
        {
            var data =
                await _eventDataService.UpdateAndReturnDataById(eventId, eventDataDto);
            return Ok(data);
        }

        [HttpPatch("{eventId}/avatar")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> UpdateAvatar(int eventId,
            [FromForm(Name = "image")] IFormFile newAvatar)
        {
            if (newAvatar == null) return UnprocessableEntity();
            var res = await _eventDataService.UpdateAvatarAsync(eventId, newAvatar);
            return Ok(res);
        }

        [HttpPatch]
        [Authorize]
        [AuthorizeEventAdmin]
        [Route("{eventId}/background")]
        public async Task<IActionResult> UpdateBackground(int eventId, [FromForm(Name =
                "image")]
            IFormFile newBackground)
        {
            if (newBackground == null) return UnprocessableEntity();
            var res =
                await _eventDataService.UpdateBackgroundAsync(eventId, newBackground);
            return Ok(res);
        }
    }
}