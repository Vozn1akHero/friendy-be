using System.Threading.Tasks;
using BE.Features.Photo.Helpers;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Photo
{
    [ApiController]
    [Route("api/user-photo")]
    public class UserPhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        private readonly IRepositoryWrapper _repository;

        public UserPhotoController(IPhotoService photoService, IRepositoryWrapper repository)
        {
            _photoService = photoService;
            _repository = repository;
        }

        [HttpDelete]
        [Authorize]
        [AuthorizeImageCreator]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var entity = _repository.UserPhoto.GetById(id);
            if (entity == null) return NotFound();
            await _photoService.DeleteUserPhotoAsync(entity);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("range")]
        public async Task<IActionResult> GetRangeAsync([FromQuery(Name = "startIndex")]
            int startIndex,
            [FromQuery(Name = "length")] int length,
            [FromQuery(Name = "userId")] int userId)
        {
            var photos =
                await _photoService.GetUserPhotoRangeAsync(userId, startIndex,
                    length);
            return Ok(photos);
        }

        [HttpGet]
        [Authorize]
        [Route("{userId}/page/{page}")]
        public async Task<IActionResult> GetRangeWithPaginationAsync(int userId,
            int page, [FromQuery(Name = "length")] int length)
        {
            var photos =
                await _photoService.GetUserPhotoRangeWithPaginationAsync(userId, page, length);
            return Ok(photos);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPhotoAsync(
            [FromForm(Name = "image")] IFormFile file,
            [FromHeader(Name = "userId")] int userId)
        {
            if (file == null) return UnprocessableEntity();
            var createdImage = await _photoService
                .AddUserPhotoAsync(userId, file);
            return Ok(createdImage);
        }
    }
}