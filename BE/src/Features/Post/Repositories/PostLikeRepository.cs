using System.Threading.Tasks;
using BE.Features.Post.Repositories;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Post
{
    public class PostLikeRepository : RepositoryBase<PostLike>, IPostLikeRepository
    {
        public PostLikeRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task CreateAsync(PostLike postLike)
        {
            Create(postLike);
            await SaveAsync();
        }

        public async Task RemoveByPostIdAsync(int postId, int userId)
        {
            var foundPostLike =
                await FindByCondition(e => e.PostId == postId && e.UserId == userId)
                    .SingleOrDefaultAsync();

            if (foundPostLike != null)
            {
                Delete(foundPostLike);
                await SaveAsync();
            }
        }

        public async Task RemoveLikesByPostIdAsync(int postId)
        {
            var foundPostLikes = await FindByCondition(e => e.PostId == postId)
                .ToListAsync();
            if (foundPostLikes.Count != 0)
            {
                foundPostLikes.ForEach(Delete);
                await SaveAsync();
            }
        }

        public bool GetPostLikedByUser(int postId, int userId)
        {
            return ExistsByCondition(e => e.PostId == postId && e.UserId == userId);
        }
    }
}