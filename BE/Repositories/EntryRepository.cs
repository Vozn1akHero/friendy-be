using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class EntryRepository : RepositoryBase<Entry>, IEntryRepository
    {
        public EntryRepository(FriendyContext friendyContext)
            : base(friendyContext)
        {

        }

        public async Task<Entry> CreateEntry(Entry entry)
        {
            Create(entry);
            await SaveAsync();
            var createdEntry = await FindByCondition(e => e == entry).SingleOrDefaultAsync();
            return createdEntry;
        }
    }
}