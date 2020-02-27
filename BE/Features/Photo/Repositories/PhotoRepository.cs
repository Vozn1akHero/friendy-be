using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Photo.Repositories
{
    public class PhotoRepository : RepositoryBase<Image>, IPhotoRepository
    {
        public PhotoRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<Image> Add(Image image)
        {
            Create(image);
            await SaveAsync();
            return image;
        }

        public async Task<int> GetAmountWithSpecificPathPattern(string pattern)
        {
            return await Task.Run(() =>
                FindByCondition(e => e.Path.StartsWith(pattern)).Count());
        }
    }
}