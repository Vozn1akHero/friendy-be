using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Repositories;
using BE.Models;
namespace BE.Features.User.Repositories
{
    public interface IUserPhotoRepository : IRepositoryBase<UserImage>
    {
        Task<IEnumerable<UserImage>> GetRangeAsync(int authorId, int startIndex, int
            length);

        Task Add(UserImage userImage);

        Task<IEnumerable<UserImage>> GetRangeWithPaginationAsync(int userId,
            int page);
    }
}