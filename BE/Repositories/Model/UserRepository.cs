﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Entities;
using BE.Helpers;
using BE.Interfaces;
using BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace BE.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(FriendyContext friendyContext)
            : base(friendyContext) { }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await FindAll()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<User> GetUser(string token)
        {
            string cutToken = token.Split(" ")[1];
            var user = await FindByCondition(o => o.Session.Token == cutToken)
                .SingleOrDefaultAsync();

            return user;
        }   
        
        public async Task<User> GetUserById(int id)
        {
            var user = await FindByCondition(o => o.Id == id)
                .SingleOrDefaultAsync();

            return user;
        }        
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await FindByCondition(o => o.Email == email)
                .SingleOrDefaultAsync();

            return user;
        }

        public async Task SetSessionId(int userId, int sessionId)
        {
            var user = await FindByCondition(o => o.Id == userId).SingleOrDefaultAsync();
            user.SessionId = sessionId;
            await SaveAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByCriteria(UsersLookUpCriteriaDto usersLookUpCriteriaDto)
        {
            var foundUsers = await FindAll().ToListAsync();
            
            if (usersLookUpCriteriaDto.Name != null)
            {
                foundUsers.RemoveAll(e =>
                    e.Name.ToLower().StartsWith(usersLookUpCriteriaDto.Name.ToLower()));
            }

            if (usersLookUpCriteriaDto.Surname != null)
            {
                foundUsers.RemoveAll(e =>
                    e.Surname.ToLower().Contains(usersLookUpCriteriaDto.Surname.ToLower()));
            }

            if (usersLookUpCriteriaDto.City != null)
            {
                foundUsers.RemoveAll(e => e.City.ToLower().Contains(usersLookUpCriteriaDto.City.ToLower()));
            }
            
            /*await FindByCondition(e => ( (usersLookUpCriteriaDto.Education != 0
                                                            && e.AdditionalInfo.EducationId == usersLookUpCriteriaDto.Education)
                                                       && (usersLookUpCriteriaDto.Gender != 0 
                                                           && e.GenderId == usersLookUpCriteriaDto.Gender)
                                                       && (usersLookUpCriteriaDto.MaritalStatus != 0  
                                                           && e.AdditionalInfo.MaritalStatusId == usersLookUpCriteriaDto.MaritalStatus)
                                                       && (usersLookUpCriteriaDto.Religion != 0 
                                                           && e.AdditionalInfo.ReligionId == usersLookUpCriteriaDto.Religion)
                                                       && (usersLookUpCriteriaDto.AlcoholOpinion != 0 
                                                           && e.AdditionalInfo.AlcoholAttitudeId == usersLookUpCriteriaDto.AlcoholOpinion)
                                                       && (usersLookUpCriteriaDto.SmokingOpinion != 0 
                                                           && e.AdditionalInfo.SmokingAttitudeId == usersLookUpCriteriaDto.SmokingOpinion));*/

            return foundUsers;
        }

        public async Task<IEnumerable<User>> GetByRange(int firstIndex, int lastIndex)
        {
            return await FindByCondition(e => e.Id >= firstIndex && e.Id <= lastIndex)
                .ToListAsync();
        }
        
        public async Task UpdateAvatar(string path, int userId)
        {
            var user = await FindByCondition(e => e.Id == userId).SingleOrDefaultAsync();
            user.Avatar = path;
            await SaveAsync();
        }

        public async Task GetUserEntries(int id)
        {
            
        }
        
        public async Task UpdateUserAsync(User dbUser)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserAsync(User user)
        {
            Delete(user);
            await SaveAsync();
        }
    }
}
