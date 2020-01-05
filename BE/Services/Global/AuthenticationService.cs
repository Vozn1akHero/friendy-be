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
        Task<User> CreateAndReturnAsync(NewUserDto user);
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
        
        public async Task<User> CreateAndReturnAsync(NewUserDto user)
        {
            string plainTextPassword = user.Password;
            user.Password = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
            var newUser = _mapper.Map<User>(user);
            var createdUser = await _repository.User.CreateAndReturnAsync(newUser);
            return createdUser;
        }

        public async Task<bool> CheckIfEmailIsAvailable(string email)
        {
            var emailAvailabilityStatus = await _friendyContext
                .User
                .SingleOrDefaultAsync(e => e.Email == email);
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

        public bool CheckUserAuthStatus(string token)
        {
            bool tokenValidityStatus = _jwtService.ValidateJwt(token);
            return tokenValidityStatus;
        }


    }
}
