using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Comment
    {
        public Comment()
        {
            CommentLike = new HashSet<CommentLike>();
            CommentRespond = new HashSet<CommentRespond>();
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Date { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CommentLike> CommentLike { get; set; }
        public virtual ICollection<CommentRespond> CommentRespond { get; set; }
    }
}
