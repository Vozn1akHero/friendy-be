using System;

namespace BE.Dtos.CommentDtos
{
    public class NewCommentDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}