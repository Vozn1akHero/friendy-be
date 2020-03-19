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
    }
}