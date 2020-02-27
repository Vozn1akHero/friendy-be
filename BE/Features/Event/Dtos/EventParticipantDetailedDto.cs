namespace BE.Features.Event.Dtos
{
    public class EventParticipantDetailedDto
    {
        public int Id { get; set; }
        public string AvatarPath { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsAdmin { get; set; }
    }
}