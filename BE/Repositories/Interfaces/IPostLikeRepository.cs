using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IPostLikeRepository : IRepositoryBase<PostLike>
    {
        Task CreateAsync(PostLike postLike);
        Task RemoveByPostIdAsync(int postId, int userId);
        Task RemoveLikesByPostIdAsync(int postId);
    }
}