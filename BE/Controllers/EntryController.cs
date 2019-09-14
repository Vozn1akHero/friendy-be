using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;
using BE.SignalR.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/entry")]
    public class EntryController : Controller
    {
        private readonly IHubContext<EntryHub> _hubContext;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public EntryController(IRepositoryWrapper repositoryWrapper, IHubContext<EntryHub> hubContext)
        {
            _repositoryWrapper = repositoryWrapper;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Authorize]
        [Route("createUserEntry")]
        public async Task<IActionResult> CreateUserEntry([FromBody] Entry entry)
        {
            var createdEntry = await _repositoryWrapper.Entry.CreateEntry(entry);
            string sessionToken = HttpContext.Request.Cookies["SESSION_TOKEN"];
            var user = await _repositoryWrapper.User.GetUser(sessionToken);
            var userEntry = new UserEntry
            {
                EntryId = createdEntry.Id,
                UserId = user.Id
            };
            await _repositoryWrapper.UserEntry.CreateUserEntry(userEntry);
            return Ok();
        }
        
        [HttpGet]
        [Authorize]
        [Route("getUserEntriesById")]
        public async Task<IActionResult> GetUserEntriesById(int id)
        {
            
            return Ok();
        }

    }
}