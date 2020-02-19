using System;
using BE.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace BE.Repositories
{
    public interface IDatabaseTransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }

    public class EntityDatabaseTransaction : IDatabaseTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public EntityDatabaseTransaction(FriendyContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}