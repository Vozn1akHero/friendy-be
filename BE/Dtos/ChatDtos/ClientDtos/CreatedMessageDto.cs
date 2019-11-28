namespace BE.Dtos.ChatDtos
{
    public class CreatedMessageDto
    {
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool IsUserAuthor { get; set; }
    }
}