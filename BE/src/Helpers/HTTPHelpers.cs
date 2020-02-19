using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace BE.Helpers
{
    public static class HTTPHelpers
    {
        public static ActionResult TextResult(HttpStatusCode statusCode, string reason)
        {
            return new ContentResult
            {
                StatusCode = (int) statusCode,
                Content = $"{reason}",
                ContentType = "text/plain"
            };
        }

        public static ActionResult JsonResult(HttpStatusCode statusCode, dynamic reason)
        {
            return new ContentResult
            {
                StatusCode = (int) statusCode,
                Content = reason,
                ContentType = "application/json"
            };
        }
    }
}