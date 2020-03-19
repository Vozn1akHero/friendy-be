using System.Threading.Tasks;
using BE.Features.Event.Services;
using BE.Helpers;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Event
{
    [ApiController]
    [Route("api/event-admins")]
    public class EventAdminsController : ControllerBase
    {
        private readonly IEventAdminService _eventAdminService;
        private readonly IRepositoryWrapper _repository;

        public EventAdminsController(IRepositoryWrapper repository,
            IEventAdminService eventAdminService)
        {
            _repository = repository;
            _eventAdminService = eventAdminService;
        }

        [HttpPost]
        [Authorize]
        [AuthorizeEventCreator]
        [Route("{id}/{userId}")]
        public async Task<IActionResult> CreateAsync(int id, int userId)
        {
            var isAdmin = _repository.EventAdmins.IsUserAdminById(id, userId);
            if (isAdmin) return UnprocessableEntity("USER IS ALREADY AN ADMIN");
            var entity = await _eventAdminService.CreateAndReturnAsync(id, userId);
            return CreatedAtAction("CreateAsync", entity);
        }

        [HttpGet]
        [Authorize]
        [AuthorizeEventCreator]
        [Route("filter/{id}")]
        public IActionResult FilterByEventIdAndKeyword([FromRoute(Name = "id")] int eventId,
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "length")] int length,
            [FromQuery(Name = "keyword")] string keyword, [FromHeader(Name = "userId")] int userId)
        {
            var foundAdmins = _eventAdminService.FilterRangeByKeywordAndEventId(eventId, keyword, page, length);
            return Ok(foundAdmins);
        }

        [HttpGet]
        [Authorize]
        [Route("is-admin/{id}")]
        public IActionResult CheckIfAdmin(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var result = _repository.EventAdmins.IsUserAdminById(id, userId);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("is-creator/{id}")]
        public async Task<IActionResult> CheckIfCreator(int id,
            [FromHeader(Name = "userId")] int userId)
        {
            var result = await _repository.Event.IsUserCreatorById(id, userId);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [AuthorizeEventCreator]
        [Route("range/{id}")]
        public IActionResult GetAdminList(int id,
            [FromQuery(Name = "page")] int page,
            [FromQuery(Name = "length")] int length)
        {
            var adminList = _eventAdminService.GetRangeByEventIdWithPagination(id, page, length);
            return Ok(adminList);
        }

        [HttpDelete]
        [Authorize]
        [AuthorizeEventCreator]
        [Route("{id}/{adminId}")]
        public async Task<IActionResult> DeleteByEventIdAndAdminId(int id, int adminId)
        {
            await _repository.EventAdmins.DeleteByEventIdAndAdminId(id, adminId);
            return Ok();
        }
    }
}