using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BE.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BE.Services.Global
{
    public class ImageProcessingService : IImageProcessingService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public ImageProcessingService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        
        public async Task<string> SaveAndReturnImagePath(IFormFile image, string basePath, int baseId)
        {
            if (image != null)
            {
                var bytes = new byte[16];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(bytes);
                }
                int imageCount = await _repositoryWrapper.Photo.GetAmountWithSpecificPathPattern(basePath);
                string fileName = Convert.ToString(baseId) + "_" + imageCount + "_" + BitConverter.ToString(bytes) + "_" + image.FileName;
                string imagePath = "wwwroot/" + basePath + "/" + fileName;
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
                using (var stream = new FileStream(imagePath, FileMode.OpenOrCreate))  
                {  
                    await image.CopyToAsync(stream);
                }

                return imagePath;
            }

            return null;
        }
    }
}