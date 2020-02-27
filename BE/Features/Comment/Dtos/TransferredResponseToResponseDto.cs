namespace BE.Features.Comment.Dtos
{
    public class TransferredResponseToResponseDto
    {
        public int PostId { get; set; }
        public int ResponseToCommentId { get; set; }
        public string Content { get; set; }
        public int MainCommentId { get; set; }
    }
}