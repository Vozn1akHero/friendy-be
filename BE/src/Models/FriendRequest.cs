namespace BE.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int ReceiverId { get; set; }

        public virtual User Author { get; set; }
        public virtual User Receiver { get; set; }
    }
}