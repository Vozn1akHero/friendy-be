using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;

namespace BE.Repositories
{
    public interface IUserPostRepository : IRepositoryBase<UserPost>
    {
        Task CreateAsync(UserPost post);
        Task<UserPost> CreateAndReturnAsync(UserPost post);
        Task RemoveByIdAsync(int id);
        Task<UserPost> GetByIdAsync(int id);
        //Task<IEnumerable<UserPostDto>> GetRangeByIdAsync(int userId, int startIndex, int length);
        Task<IEnumerable<UserPost>> GetRangeByIdAsync(int userId, int startIndex, int length);
        Task<IEnumerable<UserPost>> GetRangeByMinDateAsync(int userId, DateTime date, int length);
        Task<IEnumerable<UserPost>> GetLastByUserIdAsync(int userId, int length);
        Task<UserPost> GetByPostIdAsync(int postId);
    }
}