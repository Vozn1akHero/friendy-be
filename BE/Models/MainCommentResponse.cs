using System.Collections.Generic;

namespace BE.Models
{
    public class MainCommentResponse
    {
        public MainCommentResponse()
        {
            ResponseToMainCommentResponse = new HashSet<ResponseToMainCommentResponse>();
        }

        public int Id { get; set; }
        public int MainCommentId { get; set; }
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual MainComment MainComment { get; set; }

        public virtual ICollection<ResponseToMainCommentResponse>
            ResponseToMainCommentResponse { get; set; }
    }
}