using System.Threading.Tasks;
using BE.Dtos;
using BE.Models;
using BE.Services.Elasticsearch;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/user-data-update")]
    [ApiController]
    public class UserDataUpdateController : ControllerBase
    {
        private IUserDataIndexing _userDataIndexing;
        private IUserDataService _userDataService;

        public UserDataUpdateController(IUserDataService userDataService, IUserDataIndexing userDataIndexing)
        {
            _userDataService = userDataService;
            _userDataIndexing = userDataIndexing;
        }
        
        [HttpPut("education")]
        [Authorize]
        public async Task<IActionResult> UpdateEducation([FromBody] User user,
            [FromHeader(Name = "userId")] int userId)
        {
            await _userDataService.UpdateEducationDataById(userId, user.EducationId);
            await _userDataIndexing.UpdateEducation(userId, user.EducationId);
            return Ok();
        }
        
        [HttpPut("basic")]
        [Authorize]
        public async Task<IActionResult> UpdateBasic([FromBody] User user,
            [FromHeader(Name = "userId")] int userId)
        {
            await _userDataService.UpdateBasicDataById(userId, user.Name, user.Surname,
             user.Birthday);
            await _userDataIndexing.UpdateBasicData(userId, user);
            return Ok();
        }
        
        [HttpPut("additional")]
        [Authorize]
        public async Task<IActionResult> UpdateAdditional([FromBody] ExtendedUserDto user,
            [FromHeader(Name = "userId")] int userId)
        {
            await _userDataService.UpdateAdditionalDataById(userId,
                user.ReligionId, 
                user.AlcoholAttitudeId,
                user.SmokingAttitudeId,
                user.MaritalStatusId
            );
            await _userDataIndexing.UpdateAdditionalDataByIdAsync(userId,
                user.ReligionId, 
                user.AlcoholAttitudeId,
                user.SmokingAttitudeId,
                user.MaritalStatusId);
            return Ok();
        }
        
        [HttpPut("email")]
        [Authorize]
        public async Task<IActionResult> UpdateEmail([FromBody] User user,
            [FromHeader(Name = "userId")] int userId)
        {
            await _userDataService.UpdateEmailByIdAsync(userId, user.Email);
            return Ok();
        } 
        
        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] User user,
            [FromHeader(Name = "userId")] int userId)
        {
            await _userDataService.UpdatePasswordByIdAsync(userId, user.Password);
            return Ok();
        }
    }
}