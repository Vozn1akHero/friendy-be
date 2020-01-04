using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces.User
{
    public interface ISessionRepository : IRepositoryBase<Session>
    {
        Task<Session> CreateAndReturn(Session session);
        Task<Session> GetByConnectionId(string id);
        Task<Session> GetByUserId(int id);
        Task<Session> UpdateAndReturn(Session session);
    }
}