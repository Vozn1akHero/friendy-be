using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BE.Interfaces
{
    public interface IImageProcessingService
    {
        Task<string> SaveAndReturnImagePath(IFormFile image, string basePath, int baseId);
    }
}