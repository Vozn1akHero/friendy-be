using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Comment.Repositories
{
    public interface ICommentRepository : IRepositoryBase<Models.Comment>
    {
        Task RemovePostCommentsByPostIdAsync(int postId);

        Task AddAsync(Models.Comment comment);

        //Task<Comment> AddAndReturnAsync(c)
        Task<IEnumerable<TType>> GetMainCommentsByPostIdAsync<TType>(int
                postId,
            Expression<Func<Models.Comment, TType>> select);

        Task CreateLikeAsync(CommentLike like);
        Task UnlikeAsync(CommentLike like);
        Task LikeResponseAsync(CommentResponseLike like);

        Task UnlikeResponseByResponseIdAndUserIdAsync(int
            responseId, int userId);
    }
}