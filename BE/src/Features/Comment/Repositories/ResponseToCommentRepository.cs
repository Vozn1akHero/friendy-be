using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task CreateAsync(ResponseToComment
            responseToComment)
        {
            Create(responseToComment);
            //await SaveAsync();
        }

        public async Task<TType> GetByIdAsync<TType>(int id,
            Expression<Func<ResponseToComment, TType>> select)
        {
            return await Get(
                    e => e.Id == id, select)
                .SingleOrDefaultAsync();
        }
    }
}