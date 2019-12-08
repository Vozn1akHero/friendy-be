using System.Threading.Tasks;
using BE.CustomAttributes;
using BE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event-admins")]
    public class EventAdminsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public EventAdminsController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize]
        [Route("is-admin/{id}")]
        public async Task<IActionResult> CheckIfAdmin(int id, [FromHeader(Name = "userId")] int userId)
        {
            bool result = await _repository.EventAdmins.IsUserAdminById(id, userId);
            return Ok(result);
        }
        
        [HttpGet]
        [Authorize]
        [Route("is-creator/{id}")]
        public async Task<IActionResult> CheckIfCreator(int id, [FromHeader(Name = "userId")] int userId)
        {
            bool result = await _repository.Event.IsUserCreatorById(id, userId);
            return Ok(result);
        }
        
        [HttpGet]
        [Authorize]
        [AuthorizeEventCreator]
        [Route("all/{id}")]
        public async Task<IActionResult> GetAdminList(int id, [FromHeader(Name = "userId")] int userId)
        {
            var adminList = await _repository.EventAdmins.GetByEventIdAsync(id);
            return Ok(adminList);
        }

        [HttpDelete]
        [Authorize]
        [AuthorizeEventCreator]
        [Route("{id}")]
        public async Task<IActionResult> DeleteByEventIdAndAdminId(int id, [FromQuery(Name = "adminId")] int adminId)
        {
            await _repository.EventAdmins.DeleteByEventIdAndAdminId(id, adminId);
            return Ok();
        }
    }
}