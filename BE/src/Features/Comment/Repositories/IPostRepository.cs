using System.Threading.Tasks;

namespace BE.Features.Comment.Repositories
{
    public interface IPostRepository
    {
        Task CreateAsync(Models.Post post);
        Task RemoveByIdAsync(int id);
    }
}