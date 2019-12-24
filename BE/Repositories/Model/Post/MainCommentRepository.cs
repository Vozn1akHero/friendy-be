using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories.Interfaces.Post;
using Microsoft.EntityFrameworkCore;

namespace BE.Repositories
{
    public class MainCommentRepository : RepositoryBase<MainComment>, IMainCommentRepository
    {
        public MainCommentRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }
        
        public async Task<IEnumerable<TType>> GetAllByPostIdAsync<TType>(int postId,  
            Expression<Func<MainComment, TType>> select)
        {
            return await Get<TType>(
                    e => e.Comment.PostId == postId, select)
                .ToListAsync();
        }
        
        
    }
}