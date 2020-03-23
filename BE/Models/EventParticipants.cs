using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventParticipants
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int ParticipantId { get; set; }
        public bool? NotificationsOn { get; set; }

        public virtual Event Event { get; set; }
        public virtual User Participant { get; set; }
    }
}
