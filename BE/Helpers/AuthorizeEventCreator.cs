using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BE.Helpers
{
    public class AuthorizeEventCreator : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var repository =
                (IRepositoryWrapper) context.HttpContext.RequestServices.GetService(
                    typeof(IRepositoryWrapper));
            var eventId = Convert.ToInt32(context.HttpContext.GetRouteData().Values
                .GetValueOrDefault("id"));
            var userId = Convert.ToInt32(context.HttpContext.Request.Headers["userId"]);
            var res = repository.EventAdmins.IsUserAdminById(eventId, userId);
            if (!res) context.Result = new ForbidResult();
        }
    }
}