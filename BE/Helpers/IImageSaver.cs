using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BE.Repositories;
using Microsoft.AspNetCore.Http;

namespace BE.Helpers
{
    public interface IImageSaver
    {
        Task<string> SaveAndReturnImagePath(IFormFile image, string basePath, int baseId);
        Task<string> SaveWithSpecifiedName(IFormFile image, string imagePath);
    }

    public class ImageSaver : IImageSaver
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ImageSaver(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<string> SaveAndReturnImagePath(IFormFile image, string basePath,
            int baseId)
        {
            if (image != null)
            {
                var bytes = new byte[16];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(bytes);
                }

                var imageCount =
                    await _repositoryWrapper.Photo.GetAmountWithSpecificPathPattern(
                        basePath);
                var fileName = Convert.ToString(baseId) + "_" + imageCount + "_" +
                               BitConverter.ToString(bytes) + "_" + image.FileName;
                var imagePath = "wwwroot/" + basePath + "/" + fileName;
                using (var stream = new FileStream(imagePath, FileMode.OpenOrCreate))
                {
                    await image.CopyToAsync(stream);
                }

                return imagePath;
            }

            return null;
        }

        public async Task<string> SaveWithSpecifiedName(IFormFile image, string imagePath)
        {
            if (image != null)
            {
                var file = new FileInfo(imagePath);
                file.Directory.Create();
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return imagePath;
            }

            return null;
        }
    }
}