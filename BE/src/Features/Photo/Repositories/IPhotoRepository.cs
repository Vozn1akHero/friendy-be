using System.Threading.Tasks;
using BE.Repositories;
using BE.Models;
namespace BE.Features.Photo.Repositories
{
    public interface IPhotoRepository : IRepositoryBase<Image>
    {
        Task<Image> Add(Image image);
        Task<int> GetAmountWithSpecificPathPattern(string pattern);
    }
}