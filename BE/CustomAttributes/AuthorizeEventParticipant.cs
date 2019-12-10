using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BE.CustomAttributes
{
    public class AuthorizeEventParticipant : Attribute, IAsyncAuthorizationFilter
    {
        public AuthorizeEventParticipant()
        {
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            IRepositoryWrapper repository = (IRepositoryWrapper)context.HttpContext.RequestServices.GetService(typeof(IRepositoryWrapper));
            int eventId = Convert.ToInt32(context.HttpContext.GetRouteData().Values.GetValueOrDefault("eventId"));
            int userId = Convert.ToInt32(context.HttpContext.Request.Headers["userId"]);
            bool res = await repository.EventParticipants.IsEventParticipant(userId, eventId);
            if (!res)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}