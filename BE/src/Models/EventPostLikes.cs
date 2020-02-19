namespace BE.Models
{
    public class EventPostLikes
    {
        public int Id { get; set; }
        public int EventPostId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}