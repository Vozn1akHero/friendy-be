using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BE.Middlewares
{
    public class JWTInHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTInHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["SESSION_TOKEN"];

            if (token != null) context.Request.Headers.Append("Authorization", token);

            await _next.Invoke(context);
        }
    }
}