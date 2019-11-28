using System.Threading.Tasks;
using BE.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Services.Global
{
    public interface IAuthenticationService
    {
        Task<bool> Authenticate(string username, string password);
        Task<bool> CheckIfEmailIsAvailable(string email);
        Task Create(User user);
        Task LogOut(string token);
    }
    
    internal class AuthenticationService : IAuthenticationService
    {
        private FriendyContext _friendyContext;
        private IJwtService _jwtService;
        
        public AuthenticationService(FriendyContext friendyContext,
            IJwtService jwtService)
        {
            _friendyContext = friendyContext;
            _jwtService = jwtService;
        }
        
        public async Task Create(User user)
        {
            string plainTextPassword = user.Password;
            user.Password = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);

            await _friendyContext.User.AddAsync(user);
            await _friendyContext.SaveChangesAsync();
            
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
