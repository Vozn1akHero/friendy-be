using System.Threading.Tasks;
using BE.Helpers.CustomExceptions;
using BE.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class CommentLikeRepository : RepositoryBase<CommentLike>, ICommentLikeRepository
    {
        public CommentLikeRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<CommentLike> FindByLikeIdAndUserId(int id, int userId)
        {
            var like = await FindByCondition(e => e.Id == id && e.UserId == userId)
            .SingleOrDefaultAsync();
            return like;
        }
        
        public async Task<CommentLike> FindByCommentIdAndUserId(int commentId, int userId)
        {
            var like = await FindByCondition(e => e.CommentId == commentId && e.UserId == userId)
            .SingleOrDefaultAsync();
            return like;
        }
    }
}