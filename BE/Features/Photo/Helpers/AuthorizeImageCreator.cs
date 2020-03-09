using System;
using System.Collections.Generic;
using System.Linq;
using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BE.Features.Photo.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple =
        true)]
    public class AuthorizeImageCreator : Attribute, IAuthorizationFilter
    {
        private FriendyContext _friendyContext = new FriendyContext();
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            int imageId = Convert.ToInt32(context.HttpContext.GetRouteData().Values
                .GetValueOrDefault("id"));
            var userId = Convert.ToInt32(context.HttpContext.Request.Headers["userId"]);
            var res = _friendyContext.UserImage.Any(e => e.UserId == userId && e.ImageId == imageId);
            if (!res) context.Result = new ForbidResult();
        }
    }
}