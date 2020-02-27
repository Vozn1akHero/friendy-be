using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BE.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple =
        true)]
    public class AuthorizeEventAdminInPostController : Attribute,
        IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var repository =
                (IRepositoryWrapper) context.HttpContext.RequestServices.GetService(
                    typeof(IRepositoryWrapper));
            var eventId = Convert.ToInt32(context.HttpContext.GetRouteData().Values
                .GetValueOrDefault("eventId"));
            var userId = Convert.ToInt32(context.HttpContext.Request.Headers["userId"]);
            var res = await repository.EventAdmins.IsUserAdminById(eventId, userId);
            if (!res) context.Result = new ForbidResult();
        }
    }
}