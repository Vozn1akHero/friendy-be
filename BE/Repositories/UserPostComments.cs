using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class UserPostCommentsRepository : RepositoryBase<UserPostComments>, IUserPostCommentsRepository
    {
        public UserPostCommentsRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task RemovePostComments(int postId)
        {
            var foundPostComments = await FindByCondition(e => e.UserPostId == postId)
                .ToListAsync();
            if (foundPostComments.Count != 0)
            {
                foundPostComments.ForEach(foundPostComment =>
                {
                    Delete(foundPostComment);    
                });
                
                await SaveAsync();
            }
        }
    }
}