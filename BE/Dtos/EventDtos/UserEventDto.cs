using System;

namespace BE.Dtos.EventDtos
{
    public class UserEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string AvatarPath { get; set; }
        public byte[] Avatar { get; set; }
        public int ParticipantsAmount { get; set; }
        public DateTime Date { get; set; }
    }
}