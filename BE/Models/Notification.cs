using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Notification
    {
        public Notification()
        {
            CommentToPostNotification = new HashSet<CommentToPostNotification>();
            EventPostNotification = new HashSet<EventPostNotification>();
            ResponseToCommentNotification = new HashSet<ResponseToCommentNotification>();
        }

        public int Id { get; set; }
        public string TextContent { get; set; }
        public DateTime Date { get; set; }
        public int RecipientId { get; set; }
        public string ImagePath { get; set; }
        public bool Read { get; set; }

        public virtual User Recipient { get; set; }
        public virtual ICollection<CommentToPostNotification> CommentToPostNotification { get; set; }
        public virtual ICollection<EventPostNotification> EventPostNotification { get; set; }
        public virtual ICollection<ResponseToCommentNotification> ResponseToCommentNotification { get; set; }
    }
}
