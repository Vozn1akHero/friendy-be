using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/user-search")]
    [ApiController]
    public class UserSearchController : ControllerBase 
    {
        private IRepositoryWrapper _repository;
        
        public UserSearchController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Authorize]
        [Route("exemplary")]
        public async Task<IActionResult> GetStartingListForSearching([FromQuery(Name = "firstIndex")] int firstIndex,
            [FromQuery(Name = "lastIndex")] int lastIndex)
        {
            var users = await _repository.User.GetByRange(firstIndex, lastIndex);
            return Ok(users);
        }
        
        [HttpPost]
        [Authorize]
        [Route("with-criteria")]
        public async Task<IActionResult> GetUsersByCriteria(
            [FromBody] UsersLookUpCriteriaDto usersLookUpCriteriaDto)
        {
            var users = await _repository.User.GetUsersByCriteria(usersLookUpCriteriaDto);
            return Ok(users);
        }
    }
}