using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories.Interfaces.Post;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class MainCommentResponseRepository : RepositoryBase<MainCommentResponse>, 
    IMainCommentResponseRepository
    {
        public MainCommentResponseRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }
        
        public async Task<IEnumerable<TType>> 
            GetAllByCommentIdAsync<TType>(int commentId,
                Expression<Func<MainCommentResponse, TType>> select)
        {
            return await Get<TType>(
                    e => e.Id == commentId, select)
                .ToListAsync();
        }
    }
}