using System;

namespace BE.Dtos.EventDtos
{
    public class EventSearchDto
    {
        public string Title { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public int ParticipantsMin { get; set; }
        public int ParticipantsMax { get; set; }
        public int PriceMin { get; set; }
        public int PriceMax { get; set; }
        public DateTime Date { get; set; }
    }
}