using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string ErrorMsg { get; set; }

        public AuthResponse ErrorRes(string errorMsg)
        {
            return new AuthResponse
            {
                ErrorMsg = errorMsg
            };
        }
        
        public AuthResponse SuccessResult(string token, int userId)
        {
            return new AuthResponse
            {
                Token = token,
                UserId = userId
            };
        }
    }
}
