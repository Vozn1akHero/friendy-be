using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventPost
    {
        public EventPost()
        {
            EventComments = new HashSet<EventComments>();
        }

        public int Id { get; set; }
        public int EventId { get; set; }
        public string Content { get; set; }
        public int LikesQuantity { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }

        public virtual Event Event { get; set; }
        public virtual ICollection<EventComments> EventComments { get; set; }
    }
}
