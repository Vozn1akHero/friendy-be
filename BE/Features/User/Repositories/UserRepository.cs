using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.User.Dtos;
using BE.Helpers;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.User.Repositories
{
    public class UserRepository : RepositoryBase<Models.User>, IUserRepository
    {
        public UserRepository(FriendyContext friendyContext)
            : base(friendyContext)
        {
        }

        public async Task<IEnumerable<Models.User>> GetAllWithInterestsAsync()
        {
            return await FindAll()
                .OrderBy(x => x.Name)
                .Include(e => e.UserInterests)
                .ToListAsync();
        }

        public async Task<object> GetWithSelectedFields(int userId,
            string[] selectedFields)
        {
            var user = await FindByCondition(e => e.Id == userId).SingleOrDefaultAsync();
            return DynamicLinqStatement.ExtractSpecifiedFields(user, selectedFields);
        }

        public async Task<IEnumerable<UserBasicDto>> GetByRangeAsync(int firstIndex,
            int lastIndex)
        {
            var users =
                await FindByCondition(e => e.Id >= firstIndex && e.Id <= lastIndex)
                    .Select(e => new UserBasicDto
                    {
                        Id = e.Id,
                        Avatar = e.Avatar,
                        Birthday = e.Birthday,
                        City = e.City.Title,
                        GenderId = e.GenderId,
                        Name = e.Name,
                        Surname = e.Surname,
                        Status = e.Status
                    })
                    .ToListAsync();

            return users;
        }

        public bool EmailExistence(string email)
        {
            return ExistsByCondition(e => e.Email == email);
        }

        public async Task UpdateAvatarAsync(string path, int userId)
        {
            var user = await FindByCondition(e => e.Id == userId).SingleOrDefaultAsync();
            user.Avatar = path;
            await SaveAsync();
        }

        public async Task UpdateProfileBackgroundAsync(string path, int userId)
        {
            var user = await FindByCondition(e => e.Id == userId).SingleOrDefaultAsync();
            user.ProfileBg = path;
            await SaveAsync();
        }

        public async Task<string> GetAvatarByIdAsync(int userId)
        {
            return await FindByCondition(e => e.Id == userId)
                .Select(e => e.Avatar)
                .SingleOrDefaultAsync();
        }


        public async Task<string> GetProfileBackgroundByIdAsync(int userId)
        {
            return await FindByCondition(e => e.Id == userId)
                .Select(e => e.ProfileBg)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateBasicDataById(int id, string name, string surname,
            DateTime birthday)
        {
            var actUser = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            if (actUser != null)
            {
                actUser.Name = name;
                actUser.Surname = surname;
                actUser.Birthday = birthday;
                await SaveAsync();
            }
        }

        public async Task UpdateEducationDataById(int id, Education education)
        {
            var actUser = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            if (actUser != null)
            {
                actUser.Education = education;
                await SaveAsync();
            }
        }

        public async Task UpdateInterestsById(int id,
            IEnumerable<UserInterests> userInterests)
        {
            var actUser = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            if (actUser != null)
            {
                actUser.UserInterests = userInterests.ToList();
                await SaveAsync();
            }
        }

        public async Task<IEnumerable<UserInterests>> GetInterestsByIdAsync(int id)
        {
            return await FindByCondition(e => e.Id == id)
                .Select(e => e.UserInterests)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<IEnumerable<UserInterests>>>
            GetAllUserInterestsWithExceptionOfId(int id)
        {
            var ints = await FindByCondition(e => e.Id != id)
                .Select(e => e.UserInterests)
                .ToListAsync();
            return ints;
        }

        public async Task<Models.User> CreateAndReturnAsync(Models.User user)
        {
            if (user == null) return null;
            Create(user);
            await SaveAsync();
            return user;
        }

        public async Task<IEnumerable<TType>> GetAllAsync<TType>(Expression<Func<Models.User, TType>> selector)
        {
            return await FindAll()
                .Select(selector)
                .ToListAsync();
        }

        public async Task<TType> GetSelectedFieldsById<TType>(int userId,
            Expression<Func<Models.User, TType>> select)
        {
            return await Get(e => e.Id == userId, select).SingleOrDefaultAsync();
        }

        public async Task<Models.User> GetByIdAsync(int id)
        {
            var user = await FindByCondition(o => o.Id == id)
                .SingleOrDefaultAsync();

            return user;
        }

        public Models.User GetByEmail(string email)
        {
            return FindByCondition(o => o.Email == email)
                .SingleOrDefault();
        }
    }
}