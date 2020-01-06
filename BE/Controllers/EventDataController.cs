using System.Threading.Tasks;
using BE.CustomAttributes;
using BE.Services.Global;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-data")]
    public class EventDataController : ControllerBase
    {
        private IImageSaver _imageSaver;
        private IEventDataService _eventDataService;

        public EventDataController(IImageSaver imageSaver, IEventDataService eventDataService)
        {
            _imageSaver = imageSaver;
            _eventDataService = eventDataService;
        }

        [HttpPut]
        [Authorize]
        [AuthorizeEventAdmin]
        [Route("{id}/avatar")]
        public async Task<IActionResult> UpdateAvatar(int id, [FromForm(Name = "newAvatar")] IFormFile newAvatar)
        {
            if (newAvatar == null)
            {
                return UnprocessableEntity();
            }
            string path = $"wwwroot/EventAvatar/{id}/{newAvatar.FileName}";
            await _imageSaver
                .SaveWithSpecifiedName(newAvatar, path);
            await _eventDataService.UpdateAvatarAsync(id, path);
            return Ok(path);
        }

        [HttpPut]
        [Authorize]
        [AuthorizeEventAdmin]
        [Route("{id}/background")]
        public async Task<IActionResult> UpdateBackground(int id, [FromForm(Name = "newBackground")] IFormFile newBackground)
        {
            if (newBackground == null)
            {
                return UnprocessableEntity();
            }
            string path = $"wwwroot/EventBackground/{id}/{newBackground.FileName}";
            await _imageSaver
                .SaveWithSpecifiedName(newBackground, path);
            await _eventDataService.UpdateBackgroundAsync(id, path);
            return Ok(path);
        }
    }
}