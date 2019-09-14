using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class UserEntryRepository : RepositoryBase<UserEntry>, IUserEntryRepository
    {
        public UserEntryRepository(FriendyContext friendyContext)
            : base(friendyContext)
        {

        }

        public async Task CreateUserEntry(UserEntry entry)
        {
            Create(entry);
            await SaveAsync();
        }

        public async Task<List<UserEntry>> GetUserEntriesById(int id)
        {
            var entries = await FindByCondition(e => e.UserId == id).ToListAsync();
            return entries;
        } 
    }
}