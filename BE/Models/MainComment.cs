using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class MainComment
    {
        public MainComment()
        {
            CommentToPostNotification = new HashSet<CommentToPostNotification>();
            ResponseToComment = new HashSet<ResponseToComment>();
        }

        public int Id { get; set; }
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual ICollection<CommentToPostNotification> CommentToPostNotification { get; set; }
        public virtual ICollection<ResponseToComment> ResponseToComment { get; set; }
    }
}
