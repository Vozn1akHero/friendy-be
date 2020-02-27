namespace BE.Features.Comment.Dtos
{
    public class NewCommentDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}