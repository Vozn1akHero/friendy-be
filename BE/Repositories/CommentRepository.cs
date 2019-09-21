using BE.Interfaces.Repositories;
using BE.Models;

namespace BE.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }
    }
}