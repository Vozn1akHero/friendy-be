using System.Threading.Tasks;
using BE.Interfaces;
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
                .EventParticipants.GetExemplary(eventId));
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetRange([FromQuery(Name = "start")] int startIndex,
            [FromQuery(Name = "length")] int length, 
            [FromQuery(Name = "eventId")] int eventId)
        {
            return Ok(await _repository.EventParticipants.GetRange(eventId, startIndex, length));
        } 
    }
}