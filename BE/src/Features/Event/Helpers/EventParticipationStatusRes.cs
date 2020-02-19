namespace BE.Features.Event.Helpers
{
    public class EventParticipationStatusRes
    {
        public int EventId { get; set; }
        public int IssuerId { get; set; }
        public EventParticipationStatus EventParticipationStatus { get; set; }
    }
}