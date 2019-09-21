using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventImage
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Image { get; set; }

        public virtual Event Event { get; set; }
    }
}
