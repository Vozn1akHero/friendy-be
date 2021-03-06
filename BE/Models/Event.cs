﻿using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Event
    {
        public Event()
        {
            EventAdmins = new HashSet<EventAdmins>();
            EventBannedUsers = new HashSet<EventBannedUsers>();
            EventImage = new HashSet<EventImage>();
            EventParticipants = new HashSet<EventParticipants>();
            EventParticipationRequest = new HashSet<EventParticipationRequest>();
            EventPost = new HashSet<EventPost>();
            EventPostNotification = new HashSet<EventPostNotification>();
            UserEvents = new HashSet<UserEvents>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public decimal EntryPrice { get; set; }
        public int ParticipantsAmount { get; set; }
        public string Avatar { get; set; }
        public string Background { get; set; }
        public DateTime Date { get; set; }
        public int CreatorId { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }
        public virtual User Creator { get; set; }
        public virtual ICollection<EventAdmins> EventAdmins { get; set; }
        public virtual ICollection<EventBannedUsers> EventBannedUsers { get; set; }
        public virtual ICollection<EventImage> EventImage { get; set; }
        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
        public virtual ICollection<EventParticipationRequest> EventParticipationRequest { get; set; }
        public virtual ICollection<EventPost> EventPost { get; set; }
        public virtual ICollection<EventPostNotification> EventPostNotification { get; set; }
        public virtual ICollection<UserEvents> UserEvents { get; set; }
    }
}
