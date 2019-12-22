using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos;
using BE.Interfaces;
using BE.Models;
using BE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BE.Services.Global
{
    public interface IAuthenticationService
    {
        Task<bool> Authenticate(string username, string password);
        Task<bool> CheckIfEmailIsAvailable(string email);
        Task SetSessionIdByUserId(int id, int userId);
        Task Create(NewUserDto user);
        Task LogOut(string token);
    }
    
    internal class AuthenticationService : IAuthenticationService
    {
        private FriendyContext _friendyContext;
        private IJwtService _jwtService;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        
        public AuthenticationService(FriendyContext friendyContext,
            IJwtService jwtService, IRepositoryWrapper repository, IMapper mapper)
        {
            _friendyContext = friendyContext;
            _jwtService = jwtService;
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task Create(NewUserDto user)
        {
            string plainTextPassword = user.Password;
            user.Password = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
            var newUser = _mapper.Map<User>(user);
            await _repository.User.CreateAsync(newUser);
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
        
        public async Task<bool> Authenticate(string email, string password)
        {
            var user = await ValidateEmailAndReturnUser(email);

            if (user != null)
            {
                if (!ValidatePassword(password, user.Password))
                    return false;
                
                return true;
            }

            return false;
        }

        public async Task SetSessionIdByUserId(int id, int userId)
        {
            var user = await _friendyContext.User.SingleOrDefaultAsync(e => e.Id == userId);
            if (user != null)
            {
                user.AuthenticationSessionId = id;
                await _friendyContext.SaveChangesAsync();
            }
        }

        public bool CheckUserAuthStatus(string token)
        {
            bool tokenValidityStatus = _jwtService.ValidateJwt(token);
            return tokenValidityStatus;
        }

        public async Task LogOut(string token)
        {
            var session = await _friendyContext.AuthenticationSession.SingleOrDefaultAsync(e => e.Token == token);
            var user = await _friendyContext.User.SingleOrDefaultAsync(e => e.AuthenticationSessionId == session.Id);
            user.AuthenticationSessionId = null;
            user.Session = null;
            _friendyContext.AuthenticationSession.Remove(session);
            await _friendyContext.SaveChangesAsync();
        }

    }
}
