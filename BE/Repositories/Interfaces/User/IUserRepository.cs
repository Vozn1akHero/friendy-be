using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Models;
using Microsoft.AspNetCore.Mvc;

namespace BE.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task CreateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<Models.User>> GetAllWithInterestsAsync();
        Task<object> GetWithSelectedFields(int userId, string[] selectedFields);
        //Task<ExtendedUserDto> GetExtendedInfoById(int userId);
        Task<TType> GetSelectedFieldsById<TType>(int userId, Expression<Func<User, TType>> select);
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        //Task<IEnumerable<UserBasicDto>> GetByCriteriaAsync(UsersLookUpCriteriaDto usersLookUpCriteriaDto);
        Task<IEnumerable<UserBasicDto>> GetByRangeAsync(int firstIndex, int lastIndex);
        Task SetSessionIdAsync(int userId, int id);
        Task UpdateAvatarAsync(string path, int userId);
        Task UpdateProfileBackgroundAsync(string path, int userId);
        Task<string> GetAvatarByIdAsync(int userId);
        Task<string> GetProfileBackgroundByIdAsync(int userId);
        Task UpdateBasicDataById(int id, string name, string surname, DateTime birthday);
        Task UpdateEducationDataById(int id, Education education);
        Task UpdateInterestsById(int id, IEnumerable<UserInterests> userInterests);

        Task<IEnumerable<UserInterests>> GetInterestsById(int id);

        Task<IEnumerable<IEnumerable<UserInterests>>> 
        GetAllUserInterestsWithExceptionOfId(int id);
        //Task UpdateUserAsync(User dbUser);
    }
}
