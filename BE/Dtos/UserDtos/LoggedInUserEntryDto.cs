namespace BE.Dtos
{
    public class LoggedInUserEntryDto
    {
        public string  Content { get; set; } 
        public string Image { get; set; }
        public int CommentsQuantity  { get; set; }
        public int LikesQuantity { get; set; }
    }
}