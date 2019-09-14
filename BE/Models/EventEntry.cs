using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventEntry
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public int EventId { get; set; }

        public virtual Entry Entry { get; set; }
        public virtual Event Event { get; set; }
    }
}
