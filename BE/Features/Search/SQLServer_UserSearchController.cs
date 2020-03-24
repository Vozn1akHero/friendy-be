using System.ComponentModel;
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
        public async Task<IActionResult> GetUsersByCriteria(
            [DefaultValue(1), FromQuery(Name = "page")] int page,
            [DefaultValue(20), FromQuery(Name = "length")] int length,
            [FromBody] UsersLookUpCriteriaDto usersLookUpCriteriaDto, [FromHeader(Name 
            = "userId")] int userId)
        {
            var users = await _userSearchService.SearchByCriteriaAsync(usersLookUpCriteriaDto,
                userId, page, length);
            return Ok(users);
        }

        [HttpGet("trendy")]
        [Authorize]
        public IActionResult GetTrendyPeople([DefaultValue(1), FromQuery(Name = "page")] int page,
            [DefaultValue(20), FromQuery(Name = "length")] int length,
            [FromHeader(Name = "userId")] int userId)
        {
            var res = _userSearchService.TrendyUsers(userId, page, length);
            return Ok(res);
        }


        [HttpGet("by-keyword/{keyword}")]
        [Authorize]
        public IActionResult FindByKeywordAsync(string keyword,
            [DefaultValue(1), FromQuery(Name = "page")] int page,
            [DefaultValue(20), FromQuery(Name = "length")] int length,
            [FromHeader(Name = "userId")] int userId)
        {
            var res = _userSearchService.FindByKeywordAsync(keyword, issuerId: userId, page, length);
            return Ok(res);
        }
    }
}