using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserPostComments
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int UserPostId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual UserPost UserPost { get; set; }
    }
}
