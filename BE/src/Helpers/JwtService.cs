using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BE.Helpers
{
    public interface IJwtConf
    {
        string GenerateJwt(List<Claim> claims);
        bool ValidateJwt(string token);
        int GetUserIdFromJwt(string token);
    }

    public class JwtConf : IJwtConf
    {
        private readonly IConfiguration _configuration;

        public JwtConf(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetUserIdFromJwt(string token)
        {
            var cutToken = token.Split(" ")[1];
            var jwtSecret = _configuration.GetSection("AppSettings")
                .GetSection("JWTSecret").Value;
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims =
                handler.ValidateToken(cutToken, validations, out var tokenSecure);
            var userId =
                Convert.ToInt32(claims.Claims.SingleOrDefault(p => p.Type == "id")
                    ?.Value);
            return userId;
        }

        public bool ValidateJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration.GetValue<string>("AppSettings:JWTSecret");
            var key = Encoding.ASCII.GetBytes(secret);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(token, validationParameters,
                    out validatedToken);
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
            var jwtSecret = _configuration.GetSection("AppSettings")
                .GetSection("JWTSecret").Value;
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var createdTokenBase = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(createdTokenBase);

            return token;
        }
    }
}