namespace BE.Features.Comment.Dtos
{
    public class NewMainCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int Date { get; set; }
    }
}