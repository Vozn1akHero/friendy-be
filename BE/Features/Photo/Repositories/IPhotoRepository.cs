using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Photo.Repositories
{
    public interface IPhotoRepository : IRepositoryBase<Image>
    {
        Task<Image> Add(Image image);
        Task<int> GetAmountWithSpecificPathPattern(string pattern);
    }
}