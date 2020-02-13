using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces
{
    public interface ICommentLikeRepository : IRepositoryBase<CommentLike>
    {
        Task<CommentLike> FindByLikeIdAndUserId(int id, int userId);
        Task<CommentLike> FindByCommentIdAndUserId(int commentId, int userId);
    }
}