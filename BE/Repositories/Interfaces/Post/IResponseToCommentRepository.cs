using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces.Post
{
    public interface IResponseToCommentRepository : IRepositoryBase<ResponseToComment>
    {
        Task<IEnumerable<TType>>
            GetAllByMainCommentIdAsync<TType>(int commentId,
                Expression<Func<ResponseToComment, TType>> select);
    }
}