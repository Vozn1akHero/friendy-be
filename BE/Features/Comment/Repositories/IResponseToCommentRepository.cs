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

        void Add(ResponseToComment
            responseToComment);

        Task<TType> GetByIdAsync<TType>(int id,
            Expression<Func<ResponseToComment, TType>> selector);
        bool CheckIfLiked(int responseId, int userId);
        Task LikeAsync(CommentResponseLike commentResponseLike);
        Task UnlikeAsync(int responseId, int userId);
    }
}