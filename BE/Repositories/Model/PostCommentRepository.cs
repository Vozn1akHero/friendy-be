using System.Threading.Tasks;
using BE.Models;
using BE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class PostCommentRepository: RepositoryBase<PostComment>, IPostCommentRepository
    {
        public PostCommentRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task RemovePostCommentsByPostIdAsync(int postId)
        {
            var foundPostComments = await FindByCondition(e => e.PostId == postId)
                .ToListAsync();
            
            if (foundPostComments.Count != 0)
            {
                foundPostComments.ForEach(Delete);
                
                await SaveAsync();
            }
        }
    }
}