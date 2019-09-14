using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories
{
    public interface IEntryRepository : IRepositoryBase<Entry>
    {
        Task<Entry> CreateEntry(Entry entry);
    }
}