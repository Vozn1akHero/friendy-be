using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventPost
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int PostId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Post Post { get; set; }
    }
}
