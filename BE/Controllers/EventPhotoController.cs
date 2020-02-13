using System.Threading.Tasks;
using BE.CustomAttributes;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-photo")]
    public class EventPhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public EventPhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        [Authorize]
        [Route("range")]
        public async Task<IActionResult> GetRangeAsync([FromQuery(Name =
                "startIndex")]
            int startIndex,
            [FromQuery(Name = "length")] int length,
            [FromQuery(Name = "eventId")] int eventId)
        {
            var photos =
                await _photoService.GetEventPhotoRangeAsync(eventId, startIndex,
                    length);
            return Ok(photos);
        }
        
        [HttpGet]
        [Authorize]
        [Route("{eventId}/page/{page}")]
        public async Task<IActionResult> GetRangeAsync(int eventId, int page)
        {
            var photos =
                await _photoService.GetEventPhotosWithPaginationAsync(eventId, page);
            return Ok(photos);
        }
        
        [HttpPost]
        [Authorize]
        [AuthorizeEventAdmin]
        [Route("{id}")]
        public async Task<IActionResult> AddPhotoAsync(
            [FromRoute(Name = "id")] int id,
            [FromForm(Name = "image")] IFormFile file,
            [FromHeader(Name = "userId")] int userId)
        {
            if (file == null) return UnprocessableEntity();
            var createdImage = await _photoService.AddEventPhotoAsync
                (id, file);
            return Ok(createdImage);
        }
    }
}