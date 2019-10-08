using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Event
    {
        public Event()
        {
            EventAdmins = new HashSet<EventAdmins>();
            EventImage = new HashSet<EventImage>();
            EventParticipants = new HashSet<EventParticipants>();
            EventPost = new HashSet<EventPost>();
            UserEvents = new HashSet<UserEvents>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public decimal EntryPrice { get; set; }
        public int ParticipantsAmount { get; set; }
        public string Avatar { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<EventAdmins> EventAdmins { get; set; }
        public virtual ICollection<EventImage> EventImage { get; set; }
        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
        public virtual ICollection<EventPost> EventPost { get; set; }
        public virtual ICollection<UserEvents> UserEvents { get; set; }
    }
}
