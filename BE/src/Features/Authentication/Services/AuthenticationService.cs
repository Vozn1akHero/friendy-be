using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BE.Dtos.UserDtos;
using BE.ElasticSearch;
using BE.Features.Authentication.Helpers;
using BE.Helpers;
using BE.Helpers.CustomExceptions;
using BE.Models;
using BE.Repositories;

namespace BE.Features.Authentication.Services
{
    public interface IAuthenticationService
    {
        AuthenticationResponse Authenticate(string email, string password);
        //Task<bool> CheckIfEmailIsAvailable(string email);
        Task<Models.User> CreateAndReturnAsync(NewUserDto user);
        bool CheckUserAuthStatus(string token);
    }

    internal class AuthenticationService : IAuthenticationService
    {
        private FriendyContext _friendyContext;
        private readonly IJwtConf _jwtService;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private IUserDataIndexing _userDataIndexing;

        public AuthenticationService(FriendyContext friendyContext,
            IJwtConf jwtService,
            IRepositoryWrapper repository, 
            IMapper mapper,
            IUserDataIndexing userDataIndexing)
        {
            _friendyContext = friendyContext;
            _jwtService = jwtService;
            _repository = repository;
            _mapper = mapper;
            _userDataIndexing = userDataIndexing;
        }

        public async Task<Models.User> CreateAndReturnAsync(NewUserDto user)
        {
            var emailExistence = _repository.User.EmailExistence(user.Email);
            if (emailExistence) throw new EmailIsAlreadyTakenException();
            var plainTextPassword = user.Password;
            user.Password = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
            var newUser = _mapper.Map<Models.User>(user);
            var createdUser = await _repository.User.CreateAndReturnAsync(newUser);
            return createdUser;
        }

        public AuthenticationResponse Authenticate(string email, string password)
        {
            var user = _repository.User.GetByEmail(email);
            if (user == null)
                return new AuthenticationResponse {Result = false};
            if (!ValidatePassword(password, user.Password))
                return new AuthenticationResponse {Result = false};
            var claims = new List<Claim>
            {
                new Claim("sub", user.Name + user.Surname),
                new Claim("id", user.Id.ToString()),
                new Claim("email", email)
            };
            var token = _jwtService.GenerateJwt(claims);
            return new AuthenticationResponse {Result = true, Token = token};
        }


        public bool CheckUserAuthStatus(string token)
        {
            var cutToken = token.Split(" ")[1];
            var tokenValidityStatus = _jwtService.ValidateJwt(cutToken);
            return tokenValidityStatus;
        }

        #region private methods

        private bool ValidatePassword(string password, string realPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, realPassword);
        }

        #endregion
    }
}