using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Dtos.CommentDtos;
using BE.Interfaces.Repositories;
using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BE.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task RemovePostCommentsByPostIdAsync(int postId)
        {
            throw new System.NotImplementedException();
        }

        public async Task AddAsync(Comment comment)
        {
           Create(comment);
           await SaveAsync();
        }
        
        public async Task<IEnumerable<TType>> GetMainCommentsByPostIdAsync<TType>(int postId,  
        Expression<Func<Comment, TType>> select)
        {
            return await Get<TType>(
                    e => e.PostId == postId, select)
                .ToListAsync();
        }
    }
}