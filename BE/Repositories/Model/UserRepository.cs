﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dtos;
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

        public async Task<User> GetUserAsync(string token)
        {
            string cutToken = token.Split(" ")[1];
            var user = await FindByCondition(o => o.Session.Token == cutToken)
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
            user.SessionId = sessionId;
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
                    Avatar = _avatarConverterService.ConvertToByte(e.Avatar),
                    Birthday = e.Birthday,
                    BirthMonth = e.BirthMonth,
                    BirthYear = e.BirthYear,
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
                    Avatar = _avatarConverterService.ConvertToByte(e.Avatar),
                    Birthday = e.Birthday,
                    BirthMonth = e.BirthMonth,
                    BirthYear = e.BirthYear,
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

        public async Task<byte[]> GetAvatarByIdAsync(int userId)
        {
            string path = await FindByCondition(e => e.Id == userId)
                .Select(e => e.Avatar)
                .SingleOrDefaultAsync();
            return _avatarConverterService.ConvertToByte(path);
        }

        public async Task DeleteUserAsync(User user)
        {
            Delete(user);
            await SaveAsync();
        }
    }
}
