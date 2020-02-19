using System;
using BE.Features.Comment.Repositories;
using BE.Models;

namespace BE.Features.Comment
{
    public class UnitOfWork : IDisposable
    {
        private readonly FriendyContext _context = new FriendyContext();
        private ICommentRepository _commentRepository;
        private IResponseToCommentRepository _responseToCommentRepository;

        private bool disposed;

        public ICommentRepository CommentRepository =>
            _commentRepository ??
            (_commentRepository = new CommentRepository(_context));

        public IResponseToCommentRepository ResponseToCommentRepository =>
            _responseToCommentRepository ?? (_responseToCommentRepository =
                new ResponseToCommentRepository(_context));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool SaveChanges()
        {
            var returnValue = true;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    returnValue = false;
                    dbContextTransaction.Rollback();
                }
            }

            return returnValue;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    _context.Dispose();
            disposed = true;
        }
    }
}