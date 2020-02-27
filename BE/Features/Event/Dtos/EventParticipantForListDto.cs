namespace BE.Features.Event.Dtos
{
    public class EventParticipantForListDto
    {
        public int Id { get; set; }
        public string AvatarPath { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}