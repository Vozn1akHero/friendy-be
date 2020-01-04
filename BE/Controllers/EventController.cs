using System.Threading.Tasks;
using BE.CQRS.Commands.Event.EventCreation;
using BE.CustomAttributes;
using BE.Dtos.EventDtos;
using BE.Interfaces;
using BE.Models;
using BE.Services.Elasticsearch;
using BE.Services.Global;
using BE.Services.Model;
using MediatR;
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
        private IImageSaver _imageSaver;
        private IEventDataService _eventDataService;
        private IEventDataIndexing _eventDataIndexing;
        private IMediator _mediator;
        
        public EventController(IRepositoryWrapper repository, 
            IImageSaver imageSaver,
            IEventDataService eventDataService, 
            IEventDataIndexing eventDataIndexing,
            IMediator mediator)
        {
            _repository = repository;
            _imageSaver = imageSaver;
            _eventDataService = eventDataService;
            _eventDataIndexing = eventDataIndexing;
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Event @event, 
            [FromHeader(Name = "userId")] int userId)
        {
            //_eventDataIndexing.Create(eventData);
            await _mediator.Send(new EventCreationCommand
            {
                Event = @event,
                CreatorId = userId
            });
            return Created("api/event", @event);
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
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //var eventData = await _repository.Event.GetById(id);
            var eventData = await _eventDataService.GetDtoById(id);
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
        
        
        /*[HttpGet]
        [Authorize]
        [Route("{id}/avatar")]
        public async Task<IActionResult> GetAvatar(int id)
        {
            string path = await _repository.Event.GetAvatarPathByEventIdAsync(id);
            return Ok(path);
        }
        */

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
        
        /*[HttpGet]
        [Authorize]
        [Route("{id}/background")]
        public async Task<IActionResult> GetBackground(int id)
        {
            string path = await _repository.Event.GetAvatarPathByEventIdAsync(id);
            return Ok(path);
        }
        */

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