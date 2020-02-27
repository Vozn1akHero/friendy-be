using System.Threading.Tasks;
using BE.Features.Event.Commands.Event.EventCreation;
using BE.Features.Event.Services;
using BE.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Event
{
    [ApiController]
    [Route("api/event")]
    public class EventController : ControllerBase
    {
        private readonly IEventDataService _eventDataService;
        private readonly IUserEventsService _eventsService;
        private readonly IMediator _mediator;
        private readonly IRepositoryWrapper _repository;

        public EventController(IRepositoryWrapper repository,
            IEventDataService eventDataService,
            IMediator mediator,
            IUserEventsService eventsService)
        {
            _repository = repository;
            _eventDataService = eventDataService;
            _mediator = mediator;
            _eventsService = eventsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Models.Event @event,
            [FromHeader(Name = "userId")] int userId)
        {
            await _mediator.Send(new EventCreationCommand
            {
                Event = @event,
                CreatorId = userId
            });
            return Created("api/event", @event);
        }

        [HttpGet]
        [Authorize]
        [Route("user/active")]
        public async Task<IActionResult> GetLoggedInUserEvents(
            [FromHeader(Name = "userId")] int userId)
        {
            var events = await _eventsService.GetParticipatingByIdAsync(userId);
            return Ok(events);
        }

        [HttpGet]
        [Authorize]
        [Route("user/active/administered")]
        public async Task<IActionResult> GetLoggedInUserAdministeredEvents(
            [FromHeader(Name = "userId")] int userId)
        {
            var events = await _eventsService.GetAdministeredByIdAsync(userId);
            return Ok(events);
        }

        [HttpGet]
        [Authorize]
        [Route("closest")]
        public async Task<IActionResult> GetClosestEvents(
            [FromHeader(Name = "userId")] int userId,
            [FromQuery(Name = "length")] int length)
        {
            var events = await _eventsService.GetClosestByUserIdAsync(userId, length);
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
            var selectedFieldsArr = selectedFields.Split(",");
            var fields =
                await _repository.Event.GetWithSelectedFields(id, selectedFieldsArr);
            if (fields == null) return NotFound();
            return Ok(fields);
        }
    }
}