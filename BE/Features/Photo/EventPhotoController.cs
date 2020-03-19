using System.Threading.Tasks;
using BE.Helpers;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Photo
{
    [ApiController]
    [Route("api/event-photo")]
    public class EventPhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        private readonly IRepositoryWrapper _repository;

        public EventPhotoController(IPhotoService photoService,
            IRepositoryWrapper repository)
        {
            _photoService = photoService;
            _repository = repository;
        }
        
        [HttpDelete]
        [Authorize]
        [AuthorizeEventAdmin]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var entity = _repository.EventPhoto.GetById(id);
            if (entity == null) return NotFound();
            await _photoService.DeleteEventPhotoAsync(entity);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("{eventId}/page/{page}")]
        public async Task<IActionResult> GetRangeAsync(int eventId, int page, [FromQuery(Name = "length")] int length)
        {
            var photos =
                await _photoService.GetEventPhotosWithPaginationAsync(eventId, page, length);
            return Ok(photos);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeEventAdmin]
        [Route("{eventId}")]
        public async Task<IActionResult> AddPhotoAsync(
            [FromRoute(Name = "eventId")] int eventId,
            [FromForm(Name = "image")] IFormFile file,
            [FromHeader(Name = "userId")] int userId)
        {
            if (file == null) return UnprocessableEntity();
            var createdImage = await _photoService.AddEventPhotoAsync
                (eventId, file);
            return Ok(createdImage);
        }
    }
}