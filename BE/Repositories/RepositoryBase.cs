using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Helpers;
using BE.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace BE.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public FriendyContext FriendyContext { get; }
        
        protected RepositoryBase(FriendyContext friendyContext)
        {
            FriendyContext = friendyContext;
        }
        
        public IQueryable<TType> Get<TType>(Expression<Func<T, bool>> st, Expression<Func<T, TType>> sl)
        {
            return FriendyContext
                .Set<T>()
                .Where(st)
                .Select(sl);
        }
        
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> exp)
        {
            return FriendyContext.Set<T>()
                .Where(exp).AsNoTracking();
        }
        
        public IQueryable<T> FindAll()
        {
            return FriendyContext
                .Set<T>()
                .AsNoTracking();
        }

        public bool ExistsByCondition(Expression<Func<T, bool>> exp)
        {
            return FindAll().Any(exp);
        }

        public void Create(T ent)
        {
            FriendyContext.Set<T>().Add(ent);
        }
        
        public void CreateAll(IEnumerable<T> ent)
        {
            FriendyContext.Set<T>().AddRange(ent);
        }

        public void Update(T ent)
        {
            FriendyContext.Set<T>().Update(ent);
        }

        public void Delete(T ent)
        {
            FriendyContext.Set<T>().Remove(ent);
        }

        public void DeleteAll(Expression<Func<T, bool>> exp)
        {
            var entities = FriendyContext.Set<T>().Where(exp).AsNoTracking();
            FriendyContext.Set<T>().RemoveRange(entities);
        }
        
        public async Task SaveAsync()
        {
            await FriendyContext.SaveChangesAsync();
        }
        
        public void Save()
        {
            FriendyContext.SaveChanges();
        }
        
        public void SetAddedState<TBe>(TBe entity)
        {
            FriendyContext.Entry(entity).State = EntityState.Added;
        } 
        
        public void SetModifiedState<TBe>(TBe entity)
        {
            FriendyContext.Entry(entity).State = EntityState.Modified;
        }
        
        public void SetDeletedState<TBe>(TBe entity)
        {
            FriendyContext.Entry(entity).State = EntityState.Deleted;
        }
    }
}
