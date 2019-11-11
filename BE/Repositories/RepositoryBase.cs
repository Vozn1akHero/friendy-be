using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace BE.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        //protected RepositoryContext RepositoryContext { get; set; }
        private FriendyContext FriendyContext { get; }

        protected RepositoryBase(FriendyContext friendyContext)
        {
            FriendyContext = friendyContext;
        }

        public IQueryable<T> FindAll()
        {
            return FriendyContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return FriendyContext.Set<T>()
                .Where(expression);
        }
        
        public bool ExistsByCondition(Expression<Func<T, bool>> expression)
        {
            return FindAll().Any(expression);
        }

        public void Create(T entity)
        {
            FriendyContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            FriendyContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            FriendyContext.Set<T>().Remove(entity);
        }

        public async Task SaveAsync()
        {
            await FriendyContext.SaveChangesAsync();
        }
        
        public void Save()
        {
            FriendyContext.SaveChanges();
        } 

        public IQueryable<T> ExecuteSqlQuery(string query, List<object> parameters)
        {
            return FriendyContext.Set<T>().FromSql(query, parameters);
        }
    }
}
