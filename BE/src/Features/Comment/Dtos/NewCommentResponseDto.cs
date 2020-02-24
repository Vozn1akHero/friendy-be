namespace BE.Features.Comment.Dto
{
    public class NewCommentResponseDto
    {
        public string Content { get; set; }
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public int CommentResponseId { get; set; }
    }
}