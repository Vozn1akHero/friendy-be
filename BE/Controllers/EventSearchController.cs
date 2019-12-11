using System.Threading.Tasks;
using BE.Interfaces;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-search")]
    public class EventSearchController : ControllerBase
    {
        private IEventSearchService _eventSearchService;
        private IRepositoryWrapper _repository;
        
        public EventSearchController(IEventSearchService eventSearchService,
            IRepositoryWrapper repository)
        {
            _eventSearchService = eventSearchService;
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        [Route("{keyword}")]
        public async Task<IActionResult> SearchByKeywordAsync(string keyword, [FromHeader(Name = "userId")] int userId)
        {
            var foundEvents = await _eventSearchService.FilterByKeyword(keyword);
            return Ok(foundEvents);
        }
        
        [HttpGet]
        [Authorize]
        [Route("participating")]
        public async Task<IActionResult> FilterParticipatingEvents([FromQuery(Name = "keyword")] string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var events = await _eventSearchService.FilterParticipatingByKeywordAndUserId(userId, keyword);
            return Ok(events);
        }
        
        [HttpGet]
        [Authorize]
        [Route("administered")]
        public async Task<IActionResult> FilterAdministeredEvents([FromQuery(Name = "keyword")] string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var events = await _eventSearchService.FilterAdministeredByKeywordAndUserId(userId, keyword);
            
            return Ok(events);
        }
    }
}