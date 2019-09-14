using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BE.Dtos;
using BE.Helpers;
using BE.Interfaces;
using BE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BE.Services
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> Authenticate(string username, string password);
        Task<bool> CheckIfEmailIsAvailable(string email);
        Task Create(User user);
        Task LogOut(string token);
    }


    internal class AuthenticationService : IAuthenticationService
    {
        private IConfiguration _configuration;
        private FriendyContext _friendyContext;
        private IJwtService _jwtService;
        
        public AuthenticationService(IConfiguration configuration,
            FriendyContext friendyContext,
            IJwtService jwtService)
        {
            _configuration = configuration;
            _friendyContext = friendyContext;
            _jwtService = jwtService;
        }
        
        public async Task Create(User user)
        {
            string plainTextPassword = user.Password;
            user.Password = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);

            await _friendyContext.User.AddAsync(user);
            await _friendyContext.SaveChangesAsync();
        }

        public async Task<bool> CheckIfEmailIsAvailable(string email)
        {
            var emailAvailabilityStatus = await _friendyContext.User.SingleOrDefaultAsync(e => e.Email == email);
            return emailAvailabilityStatus == null;
        }
        
        private async Task<User> ValidateEmailAndReturnUser(string email)
        {
            return await _friendyContext.User.SingleOrDefaultAsync(x => x.Email == email);
        }

        private bool ValidatePassword(string password, string realPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, realPassword);
        }

        /*private SecurityToken GenerateJwt(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = _configuration.GetSection("AppSettings").GetSection("JWTSecret").Value;
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.CreateToken(tokenDescriptor);
        }*/

        public async Task<AuthResponse> Authenticate(string email, string password)
        {
            var authResponse = new AuthResponse();
            
            var user = await ValidateEmailAndReturnUser(email);

            if (user != null)
            {
                if(!ValidatePassword(password, user.Password))
                    return authResponse.ErrorRes("Data is incorrect");

                var token = _jwtService.GenerateJwt(user.Id.ToString());

                return authResponse.SuccessResult(token, user.Id);
            }

            return authResponse.ErrorRes("Data is incorrect");
        }
        
        public bool CheckUserAuthStatus(string token)
        {
            bool tokenValidityStatus = _jwtService.ValidateJwt(token);
            return tokenValidityStatus;
        }

        public async Task LogOut(string token)
        {
            string cutToken = token.Split(" ")[1];
            var session = await _friendyContext.Session.SingleOrDefaultAsync(e => e.Token == cutToken);
            var user = await _friendyContext.User.SingleOrDefaultAsync(e => e.SessionId == session.Id);
            user.SessionId = null;
            user.Session = null;
            _friendyContext.Session.Remove(session);
            await _friendyContext.SaveChangesAsync();
        }
        
        public async Task<User> GetUser(string token)
        {
            string cutToken = token.Split(" ")[1];
            var user = await _friendyContext.User.SingleOrDefaultAsync(o => o.Session.Token == cutToken);
            return user;
        }
    }
}
