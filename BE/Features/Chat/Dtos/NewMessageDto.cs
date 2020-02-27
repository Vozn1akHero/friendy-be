using Microsoft.AspNetCore.Http;

namespace BE.Features.Chat.Dtos
{
    public class NewMessageDto
    {
        public string Content { get; set; }
        public IFormFile File { get; set; }
    }
}