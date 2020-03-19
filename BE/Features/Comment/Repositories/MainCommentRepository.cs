using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Comment.Repositories
{
    public class MainCommentRepository : RepositoryBase<MainComment>,
        IMainCommentRepository
    {
        public MainCommentRepository(FriendyContext friendyContext) : base(friendyContext)
        {
        }

        public async Task<IEnumerable<TType>> GetAllByPostIdAsync<TType>(int postId,
            Expression<Func<MainComment, TType>> select)
        {
            return await Get(
                    e => e.Comment.PostId == postId, select)
                .ToListAsync();
        }

        public void CreateMainComment(MainComment comment)
        {
            Create(comment);
        }

        public TType FindById<TType>(int id,
            Expression<Func<MainComment, TType>> selector)
        {
            return FindByCondition(e => e.Id == id).Select(selector)
                .SingleOrDefault();
        }
    }
}