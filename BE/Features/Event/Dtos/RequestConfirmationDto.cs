namespace BE.Features.Event.Dtos
{
    public class RequestConfirmationDto
    {
        public int EventId { get; set; }
        public int IssuerId { get; set; }
        public int RequestId { get; set; }
    }
}