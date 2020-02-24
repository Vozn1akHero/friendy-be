using System;

namespace BE.Features.Event.Dto
{
    public class EventDataDto
    {
        public string Title { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public decimal EntryPrice { get; set; }
        public int ParticipantsAmount { get; set; }
        public DateTime Date { get; set; }
    }
}