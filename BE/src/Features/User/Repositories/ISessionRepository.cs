using System.Threading.Tasks;
using BE.Repositories;
using BE.Models;
namespace BE.Features.User.Repositories
{
    public interface ISessionRepository : IRepositoryBase<Session>
    {
        Task<Session> CreateAndReturn(Session session);
        Task<Session> GetByConnectionId(string id);
        //Task<Session> GetByUserId(int id);
        Task<Session> UpdateAndReturn(Session session);
    }
}