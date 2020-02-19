using System;
using System.Collections.Generic;

namespace BE.Models
{
    public class Comment
    {
        public Comment()
        {
            CommentLike = new HashSet<CommentLike>();
            MainComment = new HashSet<MainComment>();
            ResponseToCommentComment = new HashSet<ResponseToComment>();
            ResponseToCommentResponseToCommentNavigation =
                new HashSet<ResponseToComment>();
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Date { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CommentLike> CommentLike { get; set; }
        public virtual ICollection<MainComment> MainComment { get; set; }

        public virtual ICollection<ResponseToComment> ResponseToCommentComment
        {
            get;
            set;
        }

        public virtual ICollection<ResponseToComment>
            ResponseToCommentResponseToCommentNavigation { get; set; }
    }
}