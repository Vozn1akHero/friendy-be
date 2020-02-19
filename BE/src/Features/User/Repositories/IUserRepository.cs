using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Dtos.UserDtos;
using BE.Repositories;
using BE.Models;
namespace BE.Features.User.Repositories
{
    public interface IUserRepository : IRepositoryBase<Models.User>
    {
        Task<Models.User> CreateAndReturnAsync(Models.User user);
        Task<IEnumerable<Models.User>> GetAllAsync();
        Task<IEnumerable<Models.User>> GetAllWithInterestsAsync();

        Task<object> GetWithSelectedFields(int userId, string[] selectedFields);

        //Task<ExtendedUserDto> GetExtendedInfoById(int userId);
        Task<TType> GetSelectedFieldsById<TType>(int userId,
            Expression<Func<Models.User, TType>> select);

        Task<Models.User> GetByIdAsync(int id);

        Models.User GetByEmail(string email);

        //Task<IEnumerable<UserBasicDto>> GetByCriteriaAsync(UsersLookUpCriteriaDto usersLookUpCriteriaDto);
        Task<IEnumerable<UserBasicDto>> GetByRangeAsync(int firstIndex, int lastIndex);
        bool EmailExistence(string email);
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
    }
}