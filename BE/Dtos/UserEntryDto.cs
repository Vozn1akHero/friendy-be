using System;

namespace BE.Dtos
{
    public class UserEntryDto
    {
        public string  Content { get; set; } 
        public string Image { get; set; }
        public string UserAvatar { get; set; }
        public int CommentsQuantity  { get; set; }
        public int LikesQuantity { get; set; }
        public DateTime Date { get; set; }
    }
}