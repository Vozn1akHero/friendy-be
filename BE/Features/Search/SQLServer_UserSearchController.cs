using System.Threading.Tasks;
using BE.Features.Search.Services;
using BE.Features.User.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Search
{
    [ApiController]
    [Route("al-users")]
    public class SQLServer_UserSearchController : ControllerBase
    {
        private ISQLServer_UserSearchService _userSearchService;

        public SQLServer_UserSearchController(ISQLServer_UserSearchService userSearchService)
        {
            _userSearchService = userSearchService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUsersByCriteria([FromQuery(Name = "page")] int page,
            [FromBody] UsersLookUpCriteriaDto usersLookUpCriteriaDto, [FromHeader(Name 
            = "userId")] int userId)
        {
            var users = await _userSearchService.SearchByCriteriaAsync(usersLookUpCriteriaDto, 
            page, userId);
            return Ok(users);
        }

        [HttpGet("trendy")]
        [Authorize]
        public IActionResult GetTrendyPeople([FromQuery(Name = "page")] int page,
            [FromHeader(Name = "userId")] int userId)
        {
            var res = _userSearchService.TrendyUsers(userId, page);
            return Ok(res);
        }
    }
}