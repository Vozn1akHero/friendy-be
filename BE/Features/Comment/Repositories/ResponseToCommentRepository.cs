using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BE.Features.Comment.Repositories
{
    public class ResponseToCommentRepository : RepositoryBase<ResponseToComment>,
        IResponseToCommentRepository
    {
        public ResponseToCommentRepository(FriendyContext friendyContext) : base(
            friendyContext)
        {
        }

        public async Task<IEnumerable<TType>>
            GetAllByMainCommentIdAsync<TType>(int commentId,
                Expression<Func<ResponseToComment, TType>> select)
        {
            return await Get(
                    e => e.MainCommentId == commentId, select)
                .ToListAsync();
        }

        public void Add(ResponseToComment
            responseToComment)
        {
            Create(responseToComment);
        }

        public async Task<TType> GetByIdAsync<TType>(int id,
            Expression<Func<ResponseToComment, TType>> selector)
        {
            return await Get(
                    e => e.Id == id, selector)
                .SingleOrDefaultAsync();
        }

        public bool CheckIfLiked(int responseId, int userId)
        {
            return ExistsByCondition(e => e.CommentResponseLike.Any(p => p.UserId == userId && p.CommentResponseId == responseId));
        }

        public async Task LikeAsync(CommentResponseLike commentResponseLike)
        {
            var entity = await FindByCondition(e => e.ResponseToCommentId == commentResponseLike.CommentResponseId)
                .SingleOrDefaultAsync();
            entity?.CommentResponseLike.Add(commentResponseLike);
        }

        public async Task UnlikeAsync(int responseId, int userId)
        {
            var entity = await FindByCondition(e => e.ResponseToCommentId == responseId)
                .Include(e=>e.CommentResponseLike)
                .SingleOrDefaultAsync();
            var like = entity.CommentResponseLike.SingleOrDefault(e => e.UserId == userId);
            entity.CommentResponseLike.Remove(like);
        }
    }
}