using System.Threading.Tasks;
using BE.Features.User.Dtos;
using BE.Features.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.User
{
    [Route("api/user-data-update")]
    [ApiController]
    public class UserDataUpdateController : ControllerBase
    {
        private readonly IUserDataUpdateService _userDataUpdateService;
        private readonly IUserDataService _userDataService;

        public UserDataUpdateController(IUserDataService userDataService,
            IUserDataUpdateService userDataUpdateService)
        {
            _userDataService = userDataService;
            _userDataUpdateService = userDataUpdateService;
        }

        [HttpPut("education")]
        [Authorize]
        public async Task<IActionResult> UpdateEducation([FromBody] Models.User user,
            [FromHeader(Name = "userId")] int userId)
        {
            await _userDataUpdateService.UpdateEducationDataById(userId, user.EducationId);
            return Ok();
        }

        [HttpPut("basic")]
        [Authorize]
        public async Task<IActionResult> UpdateBasic([FromBody] Models.User user,
            [FromHeader(Name = "userId")] int userId)
        {
            await _userDataUpdateService.UpdateBasicDataById(userId, user.Name, user.Surname,
                user.Birthday);
            return Ok();
        }

        [HttpPut("additional")]
        [Authorize]
        public async Task<IActionResult> UpdateAdditional([FromBody] ExtendedUserDto user,
            [FromHeader(Name = "userId")] int userId)
        {
            await _userDataUpdateService.UpdateAdditionalDataById(userId,
                user.ReligionId,
                user.AlcoholAttitudeId,
                user.SmokingAttitudeId,
                user.MaritalStatusId
            );
            return Ok();
        }

        [HttpPut("email")]
        [Authorize]
        public async Task<IActionResult> UpdateEmail([FromBody] Models.User user,
            [FromHeader(Name = "userId")] int userId)
        {
            await _userDataUpdateService.UpdateEmailByIdAsync(userId, user.Email);
            return Ok();
        }

        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(
            [FromBody] NewPasswordDto newPasswordDto,
            [FromHeader(Name = "userId")] int userId)
        {
            var isOldPassCorrect =
                await _userDataService.CheckOldPasswordBeforeUpdateByUserIdAsync(userId,
                    newPasswordDto.OldPassword);
            if (isOldPassCorrect)
            {
                await _userDataUpdateService.UpdatePasswordByIdAsync(userId,
                    newPasswordDto.NewPassword);
                return Ok();
            }

            return UnprocessableEntity("PREVIOUS PASSWORD IS NOT CORRECT");
        }
    }
}