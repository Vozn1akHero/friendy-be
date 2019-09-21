using System;
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

        public async Task<List<User>> GetUsersByCriteria(UsersLookUpCriteriaDto usersLookUpCriteriaDto)
        {
            var foundUsers = await FindByCondition(e => (e.Name != "" && e.Name == usersLookUpCriteriaDto.Name)
                                                       && (e.Surname != "" && e.Surname == usersLookUpCriteriaDto.Surname))
                .ToListAsync();
            
            return foundUsers;
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
