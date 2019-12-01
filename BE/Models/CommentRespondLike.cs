using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class CommentRespondLike
    {
        public int Id { get; set; }
        public int CommentRespondId { get; set; }
        public int UserId { get; set; }

        public virtual CommentRespond CommentRespond { get; set; }
        public virtual User User { get; set; }
    }
}
