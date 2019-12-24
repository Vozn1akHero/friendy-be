using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class ResponseToComment
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int? ResponseToCommentId { get; set; }
        public int MainCommentId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual MainComment MainComment { get; set; }
        public virtual Comment ResponseToCommentNavigation { get; set; }
    }
}
