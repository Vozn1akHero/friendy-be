using Microsoft.AspNetCore.Http;

namespace BE.Dtos.ChatDtos
{
    public class NewMessageDto
    {
        public string Content { get; set; }
        public IFormFile File { get; set; }
    }
}