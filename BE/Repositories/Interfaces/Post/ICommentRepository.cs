using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Dtos.CommentDtos;
using BE.Models;

namespace BE.Interfaces.Repositories
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        Task RemovePostCommentsByPostIdAsync(int postId);
        Task AddAsync(Comment comment);

        Task<IEnumerable<TType>> GetMainCommentsByPostIdAsync<TType>(int
                postId,
            Expression<Func<Comment, TType>> select);

        Task CreateLikeAsync(CommentLike like);
        Task UnlikeAsync(CommentLike like);
        Task LikeResponseAsync(CommentResponseLike like);
        Task UnlikeResponseByResponseIdAndUserIdAsync(int 
            responseId, int userId);
    }
}