using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventParticipationRequest
    {
        public int Id { get; set; }
        public int IssuerId { get; set; }
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
        public virtual User Issuer { get; set; }
    }
}
