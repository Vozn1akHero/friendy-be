using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class ResponseToMainCommentResponse
    {
        public int Id { get; set; }
        public int MainCommentResponseId { get; set; }
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual MainCommentResponse MainCommentResponse { get; set; }
    }
}
