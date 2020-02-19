using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Comment.Repositories
{
    public interface IMainCommentResponseRepository : IRepositoryBase<MainCommentResponse>
    {
        Task<IEnumerable<TType>>
            GetAllByCommentIdAsync<TType>(int commentId,
                Expression<Func<MainCommentResponse, TType>> select);
    }
}