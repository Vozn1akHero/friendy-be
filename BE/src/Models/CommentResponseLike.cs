namespace BE.Models
{
    public class CommentResponseLike
    {
        public int Id { get; set; }
        public int CommentResponseId { get; set; }
        public int UserId { get; set; }

        public virtual ResponseToComment CommentResponse { get; set; }
        public virtual User User { get; set; }
    }
}