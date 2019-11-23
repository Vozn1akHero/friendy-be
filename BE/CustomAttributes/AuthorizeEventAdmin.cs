using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Interfaces;
using BE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BE.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeEventAdmin : Attribute, IAsyncAuthorizationFilter
    {
        //private readonly RequestDelegate _next;
        /*private readonly IRepositoryWrapper _repository;

        public AuthorizeEventAdmin(RequestDelegate next, IRepositoryWrapper repository)
        {
            _next = next;
            _repository = repository;
        }

        public async Task Invoke(HttpContext context, [FromQuery(Name = "id")] int eventId, [FromHeader(Name = "userId")] int userId)
        {
            bool res = await _repository.EventAdmins.IsUserAdminById(eventId, userId);
            if (!res)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.Headers.Clear();
            }

            await _next.Invoke(context);
        }*/
        
        public AuthorizeEventAdmin()
        {
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            IRepositoryWrapper repository = (IRepositoryWrapper)context.HttpContext.RequestServices.GetService(typeof(IRepositoryWrapper));
            int eventId = Convert.ToInt32(context.HttpContext.GetRouteData().Values.GetValueOrDefault("id"));
            int userId = Convert.ToInt32(context.HttpContext.Request.Headers["userId"]);
            bool res = await repository.EventAdmins.IsUserAdminById(eventId, userId);
            if (!res)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}