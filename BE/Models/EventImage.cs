using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventImage
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int ImageId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Image Image { get; set; }
    }
}
