using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Helpers;
using BE.Interfaces;
using BE.Models;
using BE.Repositories.RepositoryServices.Interfaces.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Nest;


namespace BE.Repositories
{
    public class UserRepository : RepositoryBase<Models.User>, IUserRepository
    {
        private IUserSearchingService _userSearchingService;
        
        public UserRepository(FriendyContext friendyContext,
            IUserSearchingService userSearchingService)
            : base(friendyContext)
        {
            _userSearchingService = userSearchingService;
        }

        public async Task<Models.User> CreateAndReturnAsync(Models.User user)
        {
            if (user == null) return null;
            Create(user);
            await SaveAsync();
            return user;
        }

        public async Task<IEnumerable<Models.User>> GetAllAsync()
        {
            return await FindAll()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Models.User>> GetAllWithInterestsAsync()
        {
            return await FindAll()
                .OrderBy(x => x.Name)
                .Include(e => e.UserInterests)
                .ToListAsync();
        }

        public async Task<object> GetWithSelectedFields(int userId, string[] selectedFields)
        {
            var user = await FindByCondition(e => e.Id == userId).SingleOrDefaultAsync();
            return DynamicLinqStatement.ExtractSpecifiedFields(user, selectedFields);
        }

        public async Task<TType> GetSelectedFieldsById<TType>(int userId, Expression<Func<Models.User, TType>> select)
        {
            return await Get<TType>(e => e.Id == userId, select).SingleOrDefaultAsync();
        }
        
        public async Task<Models.User> GetByIdAsync(int id)
        {
            var user = await FindByCondition(o => o.Id == id)
                .SingleOrDefaultAsync();

            return user;
        }      
        
        public async Task<Models.User> GetByEmailAsync(string email)
        {
            var user = await FindByCondition(o => o.Email == email)
                .SingleOrDefaultAsync();

            return user;
        }

        public async Task SetSessionIdAsync(int userId, int sessionId)
        {
            var user = await FindByCondition(o => o.Id == userId).SingleOrDefaultAsync();
            //user.SessionId = sessionId;
            await SaveAsync();
        }

        public async Task<IEnumerable<UserBasicDto>> GetByRangeAsync(int firstIndex, int lastIndex)
        {
            var users = await FindByCondition(e => e.Id >= firstIndex && e.Id <= lastIndex)
                .Select(e => new UserBasicDto
                {
                    Id = e.Id,
                    Avatar = e.Avatar,
                    Birthday = e.Birthday,
                    City = e.City,
                    GenderId = e.GenderId,
                    Name = e.Name,
                    Surname = e.Surname,
                    Status = e.Status
                })
                .ToListAsync();

            return users;
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

        public async Task UpdateBasicDataById(int id, string name, string surname, DateTime birthday)
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

        public async Task UpdateInterestsById(int id, IEnumerable<UserInterests> userInterests){
            var actUser = await FindByCondition(e => e.Id == id).SingleOrDefaultAsync();
            if (actUser != null)
            {
                actUser.UserInterests = userInterests.ToList();
                await SaveAsync();
            }
        }

        public async Task<IEnumerable<UserInterests>> GetInterestsById(int id)
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

        public async Task DeleteUserAsync(Models.User user)
        {
            Delete(user);
            await SaveAsync();
        }
    }
}
