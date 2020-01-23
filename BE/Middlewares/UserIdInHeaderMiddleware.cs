using System;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Services.Global;
using Microsoft.AspNetCore.Http;

namespace BE.Middlewares
{
    public class UserIdInHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdInHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtConf jwtConf)
        {
            string token = context.Request.Cookies["SESSION_TOKEN"];

            if (token != null)
            {
                bool tokenValidity = jwtConf.ValidateJwt(token.Split(" ")[1]);
                if (tokenValidity)
                {
                    int userId = jwtConf.GetUserIdFromJwt(token);
                    context.Request.Headers.Append("UserId", Convert.ToString(userId));
                }
            }
            
            await _next.Invoke(context);
        }
    }
}