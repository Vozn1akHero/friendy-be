using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Comment.Repositories
{
    public interface ICommentLikeRepository : IRepositoryBase<CommentLike>
    {
        void CreateLike(CommentLike commentLike);
        void DeleteLike(CommentLike commentLike);
        Task<CommentLike> FindByLikeIdAndUserId(int id, int userId);
        Task<CommentLike> FindByCommentIdAndUserId(int commentId, int userId);
    }
}