namespace BE.Models
{
    public class EventParticipants
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int ParticipantId { get; set; }

        public virtual Event Event { get; set; }
        public virtual User Participant { get; set; }
    }
}