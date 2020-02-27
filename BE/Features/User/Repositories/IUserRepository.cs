using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.User.Dtos;
using BE.Models;
using BE.Repositories;

namespace BE.Features.User.Repositories
{
    public interface IUserRepository : IRepositoryBase<Models.User>
    {
        Task<Models.User> CreateAndReturnAsync(Models.User user);
        Task<IEnumerable<TType>> GetAllAsync<TType>(Expression<Func<Models.User, TType>> selector);
        Task<object> GetWithSelectedFields(int userId, string[] selectedFields);
        Task<TType> GetSelectedFieldsById<TType>(int userId,
            Expression<Func<Models.User, TType>> select);
        Task<Models.User> GetByIdAsync(int id);
        Models.User GetByEmail(string email);
        Task<IEnumerable<UserBasicDto>> GetByRangeAsync(int firstIndex, int lastIndex);
        bool EmailExistence(string email);
        Task UpdateAvatarAsync(string path, int userId);
        Task UpdateProfileBackgroundAsync(string path, int userId);
        Task<string> GetAvatarByIdAsync(int userId);
        Task<string> GetProfileBackgroundByIdAsync(int userId);
        Task UpdateBasicDataById(int id, string name, string surname, DateTime birthday);
        Task UpdateEducationDataById(int id, Education education);
        Task UpdateInterestsById(int id, IEnumerable<UserInterests> userInterests);
        Task<IEnumerable<UserInterests>> GetInterestsByIdAsync(int id);
        Task<IEnumerable<IEnumerable<UserInterests>>>
            GetAllUserInterestsWithExceptionOfId(int id);
    }
}