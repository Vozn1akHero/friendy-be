using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos.PhotoDtos;
using BE.Interfaces;
using BE.Models;
using BE.Services.Global;
using Microsoft.AspNetCore.Http;

namespace BE.Services.Model
{
    public interface IPhotoService
    {
        Task<IEnumerable<PhotoDto>> GetUserPhotoRangeAsync(int authorId, int
            startIndex, int length);

        Task<IEnumerable<PhotoDto>> GetEventPhotoRangeAsync(int
            eventId, int startIndex, int length);
        Task<Image> AddEventPhotoAsync(int eventId, IFormFile file);
        Task<Image> AddUserPhotoAsync(int authorId, IFormFile file);
    }
    public class PhotoService : IPhotoService
    {
        private IRepositoryWrapper _repository;
        private IImageSaver _imageSaver;
        
        public PhotoService(IRepositoryWrapper repository, IImageSaver 
        imageSaver)
        {
            _repository = repository;
            _imageSaver = imageSaver;
        }

        public async Task<IEnumerable<PhotoDto>> GetUserPhotoRangeAsync(int 
        authorId, int
            startIndex, int length)
        {
            var images = await _repository.UserPhoto.GetRange(authorId,
                startIndex, length);
            var photoDtos = images.Select(e => new PhotoDto
            {
                Path = e.Image.Path
            }).ToList();
            return photoDtos;
        }
        
        public async Task<IEnumerable<PhotoDto>> GetEventPhotoRangeAsync(int 
        eventId, int startIndex, int length)
        {
            var images = await _repository.EventPhoto.GetRange(eventId,
                startIndex, length);
            var photoDtos = images.Select(e => new PhotoDto
            {
                Path = e.Image.Path
            }).ToList();
            return photoDtos;
        }

        public async Task<Image> AddEventPhotoAsync(int eventId, IFormFile file)
        {
            string path = await _imageSaver.SaveAndReturnImagePath(file,
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
            string path = await _imageSaver.SaveAndReturnImagePath(file, "UserPhotos", authorId);
            var image = new Image
            {
                Path = path,
                PublishDate = DateTime.Now
            };
            var createdImage = await _repository.Photo.Add(image);
            var userPhoto = new UserImage()
            {
                UserId = authorId,
                ImageId = createdImage.Id
            };
            await _repository.UserPhoto.Add(userPhoto);
            return createdImage;
        }
    }
}