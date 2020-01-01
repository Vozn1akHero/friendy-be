using System.Linq;
using System.Threading.Tasks;
using BE.CustomAttributes;
using BE.Interfaces;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-participant")]
    public class EventParticipantController : ControllerBase
    {
        private IRepositoryWrapper _repository;

        public EventParticipantController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet("exemplary/{eventId}")]
        public async Task<IActionResult> GetExemplary(int eventId)
        {
            return Ok(await _repository
                .EventParticipants.GetExemplaryAsync(eventId));
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetRange([FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length, 
            [FromQuery(Name = "eventId")] int eventId)
        {
            return Ok(await _repository.EventParticipants.GetRangeAsync(eventId, startIndex, length));
        }
        
        [HttpGet("range-detailed")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> GetRangeDetailed([FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length, 
            [FromQuery(Name = "eventId")] int eventId)
        {
            var filteredList = await _repository.EventParticipants.GetRangeDetailedAsync(eventId, startIndex, length);
            return Ok(filteredList);
        }

        [HttpGet("filter/{keyword}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> FilterByKeyword(string keyword,
            [FromHeader(Name = "userId")] int userId)
        {
            var filteredList = await _repository.EventParticipants.FilterByKeywordAsync(keyword);
            //var filteredListWoAdmin = filteredList.Where(e => e.Id != userId);
            return Ok(filteredList);
        }

        [HttpGet("{eventId}/current-user-status")]
        [Authorize]
        public async Task<IActionResult> IsLoggedInUserEventParticipant(int eventId, [FromHeader(Name = "userId")] int userId)
        {
            var res = await _repository.EventParticipants.IsEventParticipant(userId, eventId);
            return Ok(res);
        }

        [HttpPost("{id}/ban/{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> BanParticipant(int id,
            int eventId,
            [FromHeader(Name = "userId")] int userId)
        {
            if (id == userId)
            {
                return UnprocessableEntity();
            }

            return Ok();
        }

        [HttpDelete("{id}/leave/{eventId}")]
        [Authorize]
        [AuthorizeEventParticipant]
        public async Task<IActionResult> Leave(int id, int eventId)
        {
            await _repository.EventParticipants.Leave(id, eventId);
            return Ok();
        }
        
        [HttpDelete("{id}/remove/{eventId}")]
        [Authorize]
        [AuthorizeEventAdmin]
        public async Task<IActionResult> RemoveParticipant(int id,
            int eventId,
            [FromHeader(Name = "userId")] int userId)
        {
            if (id == userId)
            {
                return UnprocessableEntity();
            }

            return Ok();
        }
    }
}