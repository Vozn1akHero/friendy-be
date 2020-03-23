using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class CommentToPostNotification
    {
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public int MainCommentId { get; set; }

        public virtual MainComment MainComment { get; set; }
        public virtual Notification Notification { get; set; }
    }
}
