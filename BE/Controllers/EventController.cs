using System.Threading.Tasks;
using BE.CustomAttributes;
using BE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event")]
    public class EventController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private IImageProcessingService _imageProcessingService;
        
        public EventController(IRepositoryWrapper repository, 
            IImageProcessingService imageProcessingService)
        {
            _repository = repository;
            _imageProcessingService = imageProcessingService;
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
        
        [HttpGet("{id}/with-selected-fields")]
        [Authorize]
        public async Task<IActionResult> GetSelectedFields(int id, 
            [FromQuery(Name = "selectedFields")] string selectedFields)
        {
            string[] selectedFieldsArr = selectedFields.Split(",");
            var fields = await _repository.Event.GetWithSelectedFields(id, selectedFieldsArr);
            return Ok(fields);
        }
        
        
        [HttpGet]
        [Authorize]
        [Route("search/{keyword}")]
        public async Task<IActionResult> SearchByKeywordAsync(string keyword, [FromHeader(Name = "userId")] int userId)
        {
            var foundEvents =  await  _repository.Event.SearchByKeyword(keyword);
            return Ok(foundEvents);
        }
        
        [HttpGet]
        [Authorize]
        [Route("{id}/avatar")]
        public async Task<IActionResult> GetAvatar(int id)
        {
            string path = await _repository.Event.GetAvatarPathByEventIdAsync(id);
            return Ok(path);
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
            await _imageProcessingService
                .SaveWithSpecifiedName(newAvatar, path);
            await _repository.Event.UpdateAvatarAsync(path, id);
            return Ok(path);
        }
        
        [HttpGet]
        [Authorize]
        [Route("{id}/background")]
        public async Task<IActionResult> GetBackground(int id)
        {
            string path = await _repository.Event.GetAvatarPathByEventIdAsync(id);
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
            await _imageProcessingService
                .SaveWithSpecifiedName(newBackground, path);
            await _repository.Event.UpdateBackgroundAsync(path, id);
            return Ok(path);
        }
    }
}