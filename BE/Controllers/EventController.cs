using System.Threading.Tasks;
using BE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/event")]
    public class EventController : Controller
    {
        private IRepositoryWrapper _repositoryWrapper;

        public EventController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpGet]
        [Authorize]
        [Route("getExampleEvents")]
        public async Task<IActionResult> GetExampleEvents()
        {
            return Ok();
        }
        
    }
}