using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Helpers.CustomExceptions;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Comment.Repositories
{
    public class CommentRepository : RepositoryBase<Models.Comment>, ICommentRepository
    {
        public CommentRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task RemovePostCommentsByPostIdAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(Models.Comment comment)
        {
            Create(comment);
        }

        public async Task<IEnumerable<TType>> GetMainCommentsByPostIdAsync<TType>(
            int postId,
            Expression<Func<Models.Comment, TType>> select)
        {
            return await Get(
                    e => e.PostId == postId &&
                         e.MainComment.Any(d => d.CommentId == e.Id),
                    select)
                .ToListAsync();
        }

        public async Task CreateLikeAsync(CommentLike like)
        {
            var comment = await FindByCondition(e => e.Id == like.CommentId)
                .SingleOrDefaultAsync();
            comment?.CommentLike.Add(like);
            SetAddedState(like);
        }

        public async Task UnlikeAsync(CommentLike like)
        {
            var comment = await FindByCondition(e => e.Id == like.CommentId)
                .SingleOrDefaultAsync();
            comment?.CommentLike.Remove(like);
            SetDeletedState(like);
        }

        public async Task LikeResponseAsync(CommentResponseLike like)
        {
            var comment =
                await FindByCondition(e => e.ResponseToCommentComment.Any(d => d
                                                                                   .ResponseToCommentId ==
                                                                               like
                                                                                   .CommentResponseId))
                    .Include(e => e.ResponseToCommentComment)
                    .ThenInclude(e => e.CommentResponseLike)
                    .SingleOrDefaultAsync();
            var likeEx = comment.ResponseToCommentComment.SingleOrDefault(e =>
                e.ResponseToCommentId == like
                    .CommentResponseId)?.CommentResponseLike.Any(e =>
                e.UserId == like.UserId &&
                e.CommentResponseId == like.CommentResponseId);
            if (likeEx ?? true) throw new EntityIsAlreadyLiked();
            comment.ResponseToCommentComment.SingleOrDefault(e =>
                e.ResponseToCommentId == like
                    .CommentResponseId)?.CommentResponseLike.Add(like);
            SetAddedState(like);
        }

        public async Task UnlikeResponseByResponseIdAndUserIdAsync(int
            responseId, int userId)
        {
            var comment =
                await FindByCondition(e =>
                        e.ResponseToCommentComment.Any(d => d.Id == responseId))
                    .Include(e => e.ResponseToCommentComment)
                    .ThenInclude(e => e.CommentResponseLike)
                    .SingleOrDefaultAsync();
            var likeEx = comment?.ResponseToCommentComment.SingleOrDefault(e =>
                    e.Id == responseId)?.CommentResponseLike
                .SingleOrDefault(e =>
                    e.CommentResponseId == responseId && e.UserId == userId);
            if (likeEx == null) throw new EntityIsNotLikedException();
            comment.ResponseToCommentComment.SingleOrDefault(e =>
                e.Id == responseId)?.CommentResponseLike.Remove(likeEx);
            SetDeletedState(likeEx);
        }
    }
}