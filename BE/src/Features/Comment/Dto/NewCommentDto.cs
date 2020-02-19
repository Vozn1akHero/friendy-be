namespace BE.Features.Comment.Dto
{
    public class NewCommentDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}