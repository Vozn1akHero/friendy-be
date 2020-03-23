using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class ResponseToCommentNotification
    {
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
