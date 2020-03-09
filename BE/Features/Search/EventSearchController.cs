using System.Threading.Tasks;
using BE.Features.Event.Dtos;
using BE.Features.Search.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Search
{
    [ApiController]
    [Route("api/event-search")]
    public class EventSearchController : ControllerBase
    {
        private readonly IEventSearchService _eventSearchService;

        public EventSearchController(IEventSearchService eventSearchService)
        {
            _eventSearchService = eventSearchService;
        }

        [HttpGet]
        [Authorize]
        [Route("{keyword}")]
        public async Task<IActionResult> SearchByKeywordAsync(string keyword,
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "size")] int size,
            [FromHeader(Name = "userId")] int userId)
        {
            var foundEvents = await _eventSearchService.FilterAllByKeywordAsync(userId, keyword, page, size);
            return Ok(foundEvents);
        }

        [HttpGet]
        [Authorize]
        [Route("participating")]
        public async Task<IActionResult> FilterParticipatingEvents(
            [FromQuery(Name = "keyword")] string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var events =
                await _eventSearchService.FilterParticipatingByKeywordAndUserId(userId,
                    keyword);
            return Ok(events);
        }

        [HttpGet]
        [Authorize]
        [Route("administered")]
        public async Task<IActionResult> FilterAdministeredEvents(
            [FromQuery(Name = "keyword")] string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var events =
                await _eventSearchService.FilterAdministeredByKeywordAndUserId(userId,
                    keyword);
            return Ok(events);
        }

        [HttpGet]
        [Authorize]
        [Route("with-criteria")]
        public IActionResult SearchByCriteria([FromQuery(Name = "title")] string title)
        {
            var eventSearchDto = new EventSearchDto
            {
                Title = title
            };
            var events = _eventSearchService.FilterByCriteria(eventSearchDto);
            return Ok(events);
        }
    }
}