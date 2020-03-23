using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BE.Features.Notification.Helpers;
using BE.Models;

namespace BE.Features.Notification.Dto
{
    public class NotificationDto
    {
        public int RecipientId { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public string Url { get; set; }

        public static Expression<Func<Models.Notification, NotificationDto>> Selector()
        {
            return e => new NotificationDto()
            {
                RecipientId = e.RecipientId,
                Content = e.TextContent,
                ImagePath = e.ImagePath,
               
            };
        }
    }
}
