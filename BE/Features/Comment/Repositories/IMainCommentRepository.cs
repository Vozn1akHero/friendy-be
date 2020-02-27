using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Comment.Repositories
{
    public interface IMainCommentRepository : IRepositoryBase<MainComment>
    {
        Task<IEnumerable<TType>> GetAllByPostIdAsync<TType>(int postId,
            Expression<Func<MainComment, TType>> select);

        void Insert(MainComment comment);

        Task<TType> FindById<TType>(int id,
            Expression<Func<MainComment, TType>> selector);
    }
}