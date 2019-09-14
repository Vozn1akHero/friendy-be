using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Event
    {
        public Event()
        {
            EventEntry = new HashSet<EventEntry>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minutre { get; set; }
        public decimal EntryPrice { get; set; }
        public int ParticipantsAmount { get; set; }

        public virtual ICollection<EventEntry> EventEntry { get; set; }
    }
}
