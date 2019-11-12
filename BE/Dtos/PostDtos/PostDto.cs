using Microsoft.AspNetCore.Http;

namespace BE.Dtos
{
    public class PostDto
    {
        public string Content { get; set; }
        public IFormFile Image { get; set; }
        //public byte[] Image { get; set; }
    }
}