using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Features.Notification
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{userId}/{page}")]
       // [Authorize]
        public async Task<IActionResult> GetRangeByUserIdAsync(int page,
            [FromQuery(Name = "length")] int length, int userId)
        //[FromHeader(Name = "userId")] int userId
        {
            var res = await _notificationService.GetRangeAsync(userId, page, length);
            return Ok(res);
        }
    }
}