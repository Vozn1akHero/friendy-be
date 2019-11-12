using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Models;
using Microsoft.AspNetCore.Mvc;

namespace BE.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(string token);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserBasicDto>> GetUsersByCriteriaAsync(UsersLookUpCriteriaDto usersLookUpCriteriaDto);
        Task<IEnumerable<UserBasicDto>> GetByRangeAsync(int firstIndex, int lastIndex);
        Task SetSessionIdAsync(int userId, int id);
        Task UpdateAvatarAsync(string path, int userId);
        Task UpdateProfileBackgroundAsync(string path, int userId);
        Task<string> GetAvatarByIdAsync(int userId);

        Task<string> GetProfileBackgroundByIdAsync(int userId);
        //Task UpdateUserAsync(User dbUser);
    }
}
