using System;
using System.Threading.Tasks;
using BE.CustomAttributes;
using BE.Interfaces;
using BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-photo")]
    public class EventPhotoController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IImageProcessingService _imageProcessingService;

        public EventPhotoController(IRepositoryWrapper repository,
            IImageProcessingService imageProcessingService)
        {
            _repository = repository;
            _imageProcessingService = imageProcessingService;
        }  
        
        [HttpGet]
        [Authorize]
        [Route("range")]
        public async Task<IActionResult> GetRange([FromQuery(Name = "startIndex")] int startIndex,
            [FromQuery(Name = "length")] int length, 
            [FromQuery(Name = "eventId")] int eventId)
        {
            return Ok(await _repository.EventPhoto.GetRange(eventId, startIndex, length));
        }

        [HttpPost]
        [Authorize]
        [AuthorizeEventAdmin]
        [Route("{id}")]
        public async Task<IActionResult> AddPhoto([FromRoute(Name = "id")] int id, 
        [FromForm(Name = "image")] IFormFile file, 
        [FromHeader(Name = "userId")] int userId)
        {
            if (file == null)
            {
                return UnprocessableEntity();
            }
            string path = await _imageProcessingService.SaveAndReturnImagePath(file, "EventPhotos", id);
            var image = new Image
            {
                Path = path,
                PublishDate = DateTime.Now
            };
            var createdImage = await _repository.Photo.Add(image);
            await _repository.EventPhoto.Add(id, createdImage.Id);
            return Ok(createdImage);
        }
    }
}