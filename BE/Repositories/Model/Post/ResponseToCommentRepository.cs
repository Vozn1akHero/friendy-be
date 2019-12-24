using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories.Interfaces.Post;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class ResponseToCommentRepository : RepositoryBase<ResponseToComment>, IResponseToCommentRepository
    {
        public ResponseToCommentRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }
        
        public async Task<IEnumerable<TType>> 
            GetAllByMainCommentIdAsync<TType>(int commentId,
                Expression<Func<ResponseToComment, TType>> select)
        {
            return await Get<TType>(
                    e => e.MainCommentId == commentId, select)
                .ToListAsync();
        }
    }
}