using System.Threading.Tasks;

namespace BE.Repositories.Interfaces
{
    public interface IPostCommentRepository
    {
        Task RemovePostCommentsByPostIdAsync(int postId);
    }
}