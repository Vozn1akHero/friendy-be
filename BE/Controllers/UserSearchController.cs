using System.Linq;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/user-search")]
    [ApiController]
    public class UserSearchController : ControllerBase 
    {
        private IRepositoryWrapper _repository;
        private IUserSearchService _userSearchService;

        public UserSearchController(IRepositoryWrapper repository, IUserSearchService userSearchService)
        {
            _repository = repository;
            _userSearchService = userSearchService;
        }
        
        [HttpGet("exemplary")]
        [Authorize]
        public async Task<IActionResult> GetStartingListForSearching([FromQuery(Name = "firstIndex")] int firstIndex,
            [FromQuery(Name = "lastIndex")] int lastIndex,
            [FromHeader(Name = "userId")] int userId)
        {
            var users = await _repository.User.GetByRangeAsync(firstIndex, lastIndex);
            var usersCut = users.Where(e => e.Id != userId).ToHashSet();
            return Ok(usersCut);
        }
        
        [HttpPost("with-criteria")]
        [Authorize]
        public async Task<IActionResult> GetUsersByCriteria(
            [FromBody] UsersLookUpCriteriaDto usersLookUpCriteriaDto)
        {
            //var users = await _repository.User.GetUsersByCriteriaAsync(usersLookUpCriteriaDto);
            var users = _userSearchService.SearchByCriteria(usersLookUpCriteriaDto);
            return Ok(users);
        }
    }
}