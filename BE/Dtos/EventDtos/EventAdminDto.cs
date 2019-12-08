using System;

namespace BE.Dtos.EventDtos
{
    public class EventAdminDto
    {
        public int Id { get; set; }
        public bool IsEventCreator { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int GenderId { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
    }
}