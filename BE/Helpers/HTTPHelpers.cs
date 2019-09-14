using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BE.Helpers
{
    public static class HTTPHelpers
    {
        public static ActionResult TextResult(HttpStatusCode statusCode, string reason) => new ContentResult
        {
            StatusCode = (int)statusCode,
            Content = $"{reason}",
            ContentType = "text/plain"
        };
        public static ActionResult JsonResult(HttpStatusCode statusCode, dynamic reason) => new ContentResult
        {
            StatusCode = (int)statusCode,
            Content = reason,
            ContentType = "application/json"
        };
    }
}
