using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories.Interfaces.Post
{
    public interface IMainCommentResponseRepository : IRepositoryBase<MainCommentResponse>
    {
        Task<IEnumerable<TType>>
            GetAllByCommentIdAsync<TType>(int commentId,
                Expression<Func<MainCommentResponse, TType>> select);
    }
}