namespace BE.Models
{
    public class EventParticipationRequest
    {
        public int Id { get; set; }
        public int IssuerId { get; set; }
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
        public virtual User Issuer { get; set; }
    }
}