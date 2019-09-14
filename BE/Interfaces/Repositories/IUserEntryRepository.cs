using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories
{
    public interface IUserEntryRepository : IRepositoryBase<UserEntry>
    {
        Task CreateUserEntry(UserEntry entry);
    }
}