using System.Threading.Tasks;
using BE.Repositories;

namespace BE.Features.User.Services
{
    public interface IUserStatusService
    {
        //Task<bool> CheckIfOnlineByUserId(int userId);
    }

    public class UserStatusService : IUserStatusService
    {
        private readonly IRepositoryWrapper _repository;

        public UserStatusService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }


        /*public async Task<bool> CheckIfOnlineByUserId(int userId)
        {
            var session = await _repository.Session.GetByUserId(userId);
            if (session != null)
                return session.ConnectionStart != null && session.ConnectionEnd == null;
            return false;
        }*/
    }
}