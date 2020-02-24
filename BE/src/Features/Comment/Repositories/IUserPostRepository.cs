using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Comment.Repositories
{
    public interface IUserPostRepository : IRepositoryBase<UserPost>
    {
        void Add(UserPost post);
        IEnumerable<UserPost> GetRangeById(int userId, int startIndex,
            int length);
        IEnumerable<T> GetByPage<T>(int userId, int page, 
        Expression<Func<UserPost, T>> selector);
        UserPost GetById(int id);
    }
}