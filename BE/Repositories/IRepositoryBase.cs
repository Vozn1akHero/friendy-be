using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BE.Repositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<TType> Get<TType>(Expression<Func<T, bool>> st,
            Expression<Func<T, TType>> sl);

        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> exp);
        bool ExistsByCondition(Expression<Func<T, bool>> exp);
        void Create(T ent);
        void Update(T ent);
        void Delete(T ent);
        Task SaveAsync();
        void Save();
    }
}