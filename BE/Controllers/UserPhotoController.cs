using System.Threading.Tasks;
using BE.CustomAttributes;
using BE.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/user-photo")]
    public class UserPhotoController : ControllerBase
    {
        private IPhotoService _photoService;

        public UserPhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }  
        
        [HttpGet]
        [Authorize]
        [Route("range")]
        public async Task<IActionResult> GetRangeAsync([FromQuery(Name = 
                "startIndex")] int startIndex,
            [FromQuery(Name = "length")] int length, 
            [FromQuery(Name = "userId")] int userId)
        {
            var photos =
                await _photoService.GetUserPhotoRangeAsync(userId, startIndex,
                    length);
            return Ok(photos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPhotoAsync(
            [FromForm(Name = "image")] IFormFile file, 
            [FromHeader(Name = "userId")] int userId)
        {
            if (file == null)
            {
                return UnprocessableEntity();
            }
            var createdImage = await _photoService
                .AddUserPhotoAsync(userId, file);
            return Ok(createdImage);
        }
    }
}