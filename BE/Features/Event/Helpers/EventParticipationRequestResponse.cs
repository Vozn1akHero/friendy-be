namespace BE.Features.Event.Helpers
{
    public class EventParticipationRequestResponse
    {
        public EventParticipationRequestCreationResult CreationResult { get; set; }
        public object CreatedEntity { get; set; }
    }
}