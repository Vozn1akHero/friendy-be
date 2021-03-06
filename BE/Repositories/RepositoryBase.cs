﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryBase(FriendyContext friendyContext)
        {
            FriendyContext = friendyContext;
        }

        public FriendyContext FriendyContext { get; }

        public IQueryable<TType> Get<TType>(Expression<Func<T, bool>> st,
            Expression<Func<T, TType>> sl)
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
            return FriendyContext.Set<T>().Any(exp);
        }

        public void Create(T ent)
        {
            FriendyContext.Set<T>().Add(ent);
        }

        public void Update(T ent)
        {
            FriendyContext.Set<T>().Update(ent);
        }

        public void Delete(T ent)
        {
            FriendyContext.Set<T>().Remove(ent);
        }

        public async Task SaveAsync()
        {
            await FriendyContext.SaveChangesAsync();
        }

        public void Save()
        {
            FriendyContext.SaveChanges();
        }

        public void CreateAll(IEnumerable<T> ent)
        {
            FriendyContext.Set<T>().AddRange(ent);
        }

        public void DeleteAll(Expression<Func<T, bool>> exp)
        {
            var entities = FriendyContext.Set<T>().Where(exp).AsNoTracking();
            FriendyContext.Set<T>().RemoveRange(entities);
        }

    }
}