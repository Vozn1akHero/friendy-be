using BE.Models;

namespace BE.Repositories
{
    public class UnitOfWork
    {
        private readonly FriendyContext _context;

        public UnitOfWork(FriendyContext context)
        {
            _context = context;
        }

        public UnitOfWork() : this(new FriendyContext())
        {
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}