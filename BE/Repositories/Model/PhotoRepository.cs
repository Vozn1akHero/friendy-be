using System;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories.Interfaces;

namespace BE.Repositories
{
    public class PhotoRepository : RepositoryBase<Image>, IPhotoRepository
    {
        public PhotoRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<Image> Add(string path)
        {
            var image = new Image
            {
                Path = path,
                PublishDate = DateTime.Now
            };
            Create(image);
            await SaveAsync();
            return image;
        }
    }
}