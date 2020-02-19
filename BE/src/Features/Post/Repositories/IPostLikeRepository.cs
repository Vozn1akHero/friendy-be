using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Post.Repositories
{
    public interface IPostLikeRepository : IRepositoryBase<PostLike>
    {
        Task CreateAsync(PostLike postLike);
        Task RemoveByPostIdAsync(int postId, int userId);
        Task RemoveLikesByPostIdAsync(int postId);
        bool GetPostLikedByUser(int postId, int userId);
    }
}