using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Comment.Repositories
{
    public interface IUserPostRepository : IRepositoryBase<UserPost>
    {
        void Add(UserPost post);
        Task<IEnumerable<UserPost>> GetRangeByIdAsync(int userId, int startIndex,
            int length);
        Task<IEnumerable<UserPost>> GetRangeByMinDateAsync(int userId, DateTime date,
            int length);
        Task<IEnumerable<UserPost>> GetLastByUserIdAsync(int userId, int length);
        UserPost GetById(int id);
    }
}