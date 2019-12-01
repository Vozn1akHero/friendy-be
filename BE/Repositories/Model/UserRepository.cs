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


namespace BE.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private IUserSearchingService _userSearchingService;
        private IAvatarConverterService _avatarConverterService;
        
        public UserRepository(FriendyContext friendyContext,
            IAvatarConverterService avatarConverterService,
            IUserSearchingService userSearchingService)
            : base(friendyContext)
        {
            _userSearchingService = userSearchingService;
            _avatarConverterService = avatarConverterService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await FindAll()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<object> GetWithSelectedFields(int userId, string[] selectedFields)
        {
            var user = await FindByCondition(e => e.Id == userId).SingleOrDefaultAsync();
            return DynamicLinqStatement.ExtractSpecifiedFields(user, selectedFields);
        }

        public async Task<ExtendedUserDto> GetExtendedInfoById(int userId)
        {
            return await FindByCondition(e => e.Id == userId).Select(e => new ExtendedUserDto {
                    Id = e.Id,
                    City = e.City,
                    Name = e.Name,
                    Surname = e.Surname,
                    GenderId = e.Gender.Id,
                    Birthday = e.Birthday,
                    Avatar = e.Avatar,
                    EducationId = e.AdditionalInfo.Education.Id,
                    MaritalStatusId = e.AdditionalInfo.MaritalStatus.Id,
                    ReligionId = e.AdditionalInfo.Religion.Id,
                    AlcoholAttitudeId = e.AdditionalInfo.AlcoholAttitude.Id,
                    SmokingAttitudeId = e.AdditionalInfo.SmokingAttitude.Id,
                    UserInterests = e.AdditionalInfo.UserInterests.Select(b => new
                    {
                        b.Interest.Id,
                        b.Interest.Title
                    })
                }).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserAsync(string token)
        {
            string cutToken = token.Split(" ")[1];
            var user = await FindByCondition(o => o.AuthenticationSession.Token == cutToken)
                .SingleOrDefaultAsync();
            return user;
        }   
        
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await FindByCondition(o => o.Id == id)
                .SingleOrDefaultAsync();

            return user;
        }      
        
        public async Task<User> GetUserByEmailAsync(string email)
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

        public async Task<IEnumerable<UserBasicDto>> GetUsersByCriteriaAsync(UsersLookUpCriteriaDto usersLookUpCriteriaDto)
        {
            var users = await FindAll()
                .Include(e => e.AdditionalInfo)
                .Include(e => e.AdditionalInfo.AlcoholAttitude)
                .Include(e => e.AdditionalInfo.Education)
                .Include(e => e.AdditionalInfo.MaritalStatus)
                .Include(e => e.AdditionalInfo.Religion)
                .Include(e => e.AdditionalInfo.SmokingAttitude)
                .Select(e => new UserLookUpModelDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname,
                    City = e.City,
                    EducationId = e.AdditionalInfo.EducationId,
                    GenderId = e.GenderId,
                    MaritalStatusId = e.AdditionalInfo.MaritalStatusId,
                    ReligionId = e.AdditionalInfo.ReligionId,
                    AlcoholAttitudeId = e.AdditionalInfo.AlcoholAttitudeId,
                    SmokingAttitudeId = e.AdditionalInfo.SmokingAttitudeId,
                    UserInterests = e.AdditionalInfo.UserInterests.Select(interest => interest.Interest.Title)
                })
                .ToListAsync();
            
            var filteredUsers = _userSearchingService.Filter(users, usersLookUpCriteriaDto);
            var filteredUsersIds = filteredUsers.Select(e => e.Id);
            return await FindByCondition(e => filteredUsersIds.Contains(e.Id))
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

        public async Task DeleteUserAsync(User user)
        {
            Delete(user);
            await SaveAsync();
        }
    }
}
