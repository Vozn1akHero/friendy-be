namespace BE.Models
{
    public class UserInterests
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int InterestId { get; set; }
        public int Wage { get; set; }

        public virtual Interest Interest { get; set; }
        public virtual User User { get; set; }
    }
}