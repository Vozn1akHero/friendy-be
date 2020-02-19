namespace BE.Features.Event.Dto
{
    public class RequestConfirmationDto
    {
        public int EventId { get; set; }
        public int IssuerId { get; set; }
        public int RequestId { get; set; }
    }
}