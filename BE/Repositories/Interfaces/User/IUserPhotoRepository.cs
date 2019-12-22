using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos.PhotoDtos;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces.User
{
    public interface IUserPhotoRepository : IRepositoryBase<UserImage>
    {
        Task<IEnumerable<UserImage>> GetRange(int authorId, int startIndex, int 
        length);
        Task Add(UserImage userImage);
        Task<int> GetPicturesAmountByEventId(int authorId);
    }
}