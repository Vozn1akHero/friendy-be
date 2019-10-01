using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class UserPostLikesRepository : RepositoryBase<UserPostLikes>, IUserPostLikesRepository
    {
        public UserPostLikesRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task CreatePostLike(UserPostLikes userPostLikes)
        {
            Create(userPostLikes);
            await SaveAsync();
        }

        public async Task RemovePostLike(UserPostLikes userPostLikes)
        {
            var foundPostLike = await FindByCondition(e => e.UserPostId == userPostLikes.Id)
                .SingleOrDefaultAsync();
            if (foundPostLike != null)
            {
                Delete(foundPostLike);
                await SaveAsync();
            }
        }

        public async Task RemovePostLikes(int postId)
        {
            var foundPostLikes = await FindByCondition(e => e.UserPostId == postId)
                .ToListAsync();
            if (foundPostLikes.Count != 0)
            {
                foundPostLikes.ForEach(userPostLike =>
                {
                    Delete(userPostLike);
                });
                await SaveAsync();
            }
        }
    }
}