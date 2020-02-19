using System.Threading.Tasks;
using BE.Features.Comment.Repositories;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Post
{
    public class PostRepository : RepositoryBase<Models.Post>, IPostRepository
    {
        public PostRepository(FriendyContext friendyContext)
            : base(friendyContext)
        {
        }

        public async Task RemoveByIdAsync(int id)
        {
            var post = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            Delete(post);
            await SaveAsync();
        }

        public async Task CreateAsync(Models.Post post)
        {
            Create(post);
            await SaveAsync();
        }
    }
}