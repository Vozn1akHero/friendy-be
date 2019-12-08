using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BE.CustomAttributes
{
    public class AuthorizeEventCreator : Attribute, IAsyncAuthorizationFilter
    {
        public AuthorizeEventCreator()
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