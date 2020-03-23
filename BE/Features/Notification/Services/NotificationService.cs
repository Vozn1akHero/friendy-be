using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using BE.Features.Notification.Dto;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Features.Notification
{
    public interface INotificationService
    {
        Task<IEnumerable<Models.Notification>> GetRangeAsync(int recipientId, int page, int length);
    }

    public class NotificationService : INotificationService
    {
        private readonly FriendyContext _friendyContext;

        public NotificationService(FriendyContext friendyContext)
        {
            _friendyContext = friendyContext;
        }

        public async Task<IEnumerable<Models.Notification>> GetRangeAsync(int recipientId, int page, int length)
        {
            var result = await _friendyContext.Notification
                .Where(e=>e.RecipientId == recipientId)
                //.Select(NotificationDto.Selector())
                .Include(e => e.EventPostNotification)
                .Include(e=>e.CommentToPostNotification)
                .Include(e=>e.ResponseToCommentNotification)
                .ToListAsync();
            return result;
        }
    }
}
