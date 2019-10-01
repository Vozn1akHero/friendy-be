using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IUserPostLikesRepository : IRepositoryBase<UserPostLikes>
    {
        Task CreatePostLike(UserPostLikes userPostLikes);
        Task RemovePostLike(UserPostLikes userPostLikes);
        Task RemovePostLikes(int postId);
    }
}