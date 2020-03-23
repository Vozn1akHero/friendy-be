using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Models;

namespace BE.Features.Notification.Dto
{
    public class EventPostNotificationDto
    {
        public int EventId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }

        public static Expression<Func<EventPostNotification, EventPostNotificationDto>> Selector()
        {
            return e => new EventPostNotificationDto()
            {
                //EventId = e.SenderId,
                ReceiverId = e.Notification.RecipientId,
                Content = e.Notification.TextContent
            };
        }
    }
}
