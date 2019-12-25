using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(FriendyContext friendyContext)
            : base(friendyContext)
        { }

        public async Task CreateAsync(Post post)
        {
            Create(post);
            await SaveAsync();
        }

        public async Task RemoveByIdAsync(int id)
        {
            var post = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            Delete(post);
            await SaveAsync();
        }
    }
}