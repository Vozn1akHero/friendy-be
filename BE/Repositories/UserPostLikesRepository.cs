using System.Threading.Tasks;
using BE.Interfaces.Repositories;
using BE.Models;

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
    }
}