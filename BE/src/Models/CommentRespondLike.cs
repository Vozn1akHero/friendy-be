namespace BE.Models
{
    public class CommentRespondLike
    {
        public int Id { get; set; }
        public int CommentRespondId { get; set; }
        public int UserId { get; set; }

        public virtual CommentRespond CommentRespond { get; set; }
        public virtual User User { get; set; }
    }
}