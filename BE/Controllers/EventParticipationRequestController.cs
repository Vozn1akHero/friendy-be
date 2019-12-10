using System.Threading.Tasks;
using BE.Dtos.EventDtos;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-participation-request")]
    public class EventParticipationRequestController : ControllerBase
    {
        private RepositoryWrapper _repository;

        public EventParticipationRequestController(RepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] EventParticipationRequestDto eventParticipationRequestDto)
        {
            var newRequest = new EventParticipationRequest
            {
                IssuerId = eventParticipationRequestDto.IssuerId,
                EventId = eventParticipationRequestDto.EventId
            };
            await _repository.EventParticipationRequest.CreateAsync(newRequest);
            return Created($"api/event-participation-request/{newRequest.EventId}/{newRequest.IssuerId}", newRequest);
        }

        [HttpDelete("{eventId}/{issuerId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int issuerId, int eventId)
        {
            var newRequest = new EventParticipationRequest
            {
                IssuerId = issuerId,
                EventId = eventId
            };
            await _repository.EventParticipationRequest.DeleteAsync(newRequest);
            return Ok();
        }

        [HttpGet("status/{eventId}/{issuerId}")]
        public async Task<IActionResult> RequestStatus(int issuerId, int eventId)
        {
            bool res = await _repository.EventParticipationRequest.GetStatus(issuerId, eventId);
            return Ok(res);
        }
    }
}