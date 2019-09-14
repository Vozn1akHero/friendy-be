using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BE.Entities;
using BE.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        //protected RepositoryContext RepositoryContext { get; set; }
        protected FriendyContext FriendyContext { get; set; }

        public RepositoryBase(FriendyContext friendyContext)
        {
            this.FriendyContext = friendyContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.FriendyContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.FriendyContext.Set<T>()
                .Where(expression);
        }

        public void Create(T entity)
        {
            this.FriendyContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.FriendyContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.FriendyContext.Set<T>().Remove(entity);
        }

        public async Task SaveAsync()
        {
            await this.FriendyContext.SaveChangesAsync();
        }
    }
}
