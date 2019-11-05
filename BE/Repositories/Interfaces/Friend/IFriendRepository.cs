using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface IFriendRepository : IRepositoryBase<Friend>
    {
        //Task<List<Friend>> FindAllByUserId(int id);
        Task<Friend> GetByUserId(int userId);
    }
}