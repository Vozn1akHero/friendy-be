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
        Task<User> GetUser(string token);
        Task<User> GetUserById(int id);
        Task SetSessionId(int userId, int id);
        
        Task UpdateUserAsync(User dbUser);
        Task DeleteUserAsync(User user);
    }
}
