using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Features.Photo.Dtos;
using BE.Helpers;
using BE.Models;
using BE.Repositories;
using Microsoft.AspNetCore.Http;

namespace BE.Features.Photo
{
    public interface IPhotoService
    {
        Task<IEnumerable<UserPhotoDto>> GetUserPhotoRangeWithPaginationAsync(int userId,
            int page, int length);

        Task<IEnumerable<EventPhotoDto>> GetEventPhotosWithPaginationAsync(int
            eventId, int page, int length);

        Task<Image> AddEventPhotoAsync(int eventId, IFormFile file);
        Task<Image> AddUserPhotoAsync(int authorId, IFormFile file);
        Task DeleteUserPhotoAsync(UserImage userImage);
        Task DeleteEventPhotoAsync(EventImage image);
    }

    public class PhotoService : IPhotoService
    {
        private readonly IImageSaver _imageSaver;
        private readonly IRepositoryWrapper _repository;

        public PhotoService(IRepositoryWrapper repository, IImageSaver
            imageSaver)
        {
            _repository = repository;
            _imageSaver = imageSaver;
        }

        public async Task<IEnumerable<UserPhotoDto>> GetUserPhotoRangeWithPaginationAsync(int
            userId, int page, int length)
        {
            return await _repository.UserPhoto
                .GetRangeWithPaginationAsync(userId, page, length, UserPhotoDto.Selector);
        }

        public async Task<IEnumerable<EventPhotoDto>> GetEventPhotosWithPaginationAsync(int
            eventId, int page, int length)
        {
            return await _repository.EventPhoto.SelectWithPaginationAsync(eventId,
                page, length, EventPhotoDto.Selector); ;
        }

        public async Task<Image> AddEventPhotoAsync(int eventId, IFormFile file)
        {
            var path = await _imageSaver.SaveAndReturnImagePath(file,
                "EventPhotos", eventId);
            var image = new Image
            {
                Path = path,
                PublishDate = DateTime.Now
            };
            var createdImage = await _repository.Photo.Add(image);
            await _repository.EventPhoto.Add(eventId, createdImage.Id);
            return createdImage;
        }

        public async Task<Image> AddUserPhotoAsync(int authorId, IFormFile file)
        {
            var path =
                await _imageSaver.SaveAndReturnImagePath(file, "UserPhotos", authorId);
            var image = new Image
            {
                Path = path,
                PublishDate = DateTime.Now
            };
            var userPhoto = new UserImage
            {
                UserId = authorId,
                Image = image
            };
            await _repository.UserPhoto.Add(userPhoto);
            return image;
        }

        public async Task DeleteUserPhotoAsync(UserImage userImage)
        {
            _repository.UserPhoto.DeleteByEntity(userImage);
            await _repository.SaveAsync();
        }

        public async Task DeleteEventPhotoAsync(EventImage image)
        {
            _repository.EventPhoto.DeleteByEntity(image);
            await _repository.SaveAsync();
        }
    }
}