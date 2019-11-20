using System;
using System.IO;
using System.Threading.Tasks;
using BE.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BE.Services.Global
{
    public class ImageProcessingService : IImageProcessingService
    {
        public async Task<string> SaveAndReturnImagePath(IFormFile image, string basePath, int baseId)
        {
            if (image != null)
            {
                int rand = new Random().Next();
                string fileName = Convert.ToString(baseId) + "_" + rand + image.FileName;
                string imagePath = "wwwroot/" + basePath + "/" + fileName;
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