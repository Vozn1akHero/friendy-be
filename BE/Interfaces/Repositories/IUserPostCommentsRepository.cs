using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IUserPostCommentsRepository : IRepositoryBase<UserPostComments>
    {
        Task RemovePostComments(int postId);
    }
}