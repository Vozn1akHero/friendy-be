using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Comment.Repositories
{
    public interface IResponseToCommentRepository : IRepositoryBase<ResponseToComment>
    {
        Task<IEnumerable<TType>>
            GetAllByMainCommentIdAsync<TType>(int commentId,
                Expression<Func<ResponseToComment, TType>> select);

        Task CreateAsync(ResponseToComment
            responseToComment);

        Task<TType> GetByIdAsync<TType>(int id,
            Expression<Func<ResponseToComment, TType>> select);
    }
}