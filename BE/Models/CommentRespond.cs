using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class CommentRespond
    {
        public CommentRespond()
        {
            CommentRespondLike = new HashSet<CommentRespondLike>();
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CommentRespondLike> CommentRespondLike { get; set; }
    }
}
