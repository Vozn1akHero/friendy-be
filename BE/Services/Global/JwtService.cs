using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BE.Interfaces;

namespace BE.Helpers
{
    public class JwtService : IJwtService
    {
        private IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetUserIdFromJwt(string token)
        {
            string cutToken = token.Split(" ")[1];
            var jwtSecret = _configuration.GetSection("AppSettings").GetSection("JWTSecret").Value;
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(cutToken, validations, out var tokenSecure);
            int userId = Convert.ToInt32(claims.Claims.SingleOrDefault(p => p.Type == "id")?.Value);
            return userId;
        }

        public bool ValidateJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("AppSettings:JWTSecret"));
            TokenValidationParameters validationParameters = new TokenValidationParameters{
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return false;
            }

            return validatedToken != null;
        }

        public string GenerateJwt(List<Claim> claims)
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

            var createdTokenBase = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(createdTokenBase);

            return token;
        }
    }
}
