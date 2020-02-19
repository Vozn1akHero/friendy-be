namespace BE.Models
{
    public class UserImage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ImageId { get; set; }

        public virtual Image IdNavigation { get; set; }
        public virtual User User { get; set; }
    }
}