using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces
{
    public interface IPhotoRepository : IRepositoryBase<Image>
    {
        Task<Image> Add(Image image);
        Task<int> GetAmountWithSpecificPathPattern(string pattern);
    }
}