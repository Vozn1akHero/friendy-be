using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventAdmins
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }

        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}
