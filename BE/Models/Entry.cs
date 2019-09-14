using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Entry
    {
        public Entry()
        {
            EventEntry = new HashSet<EventEntry>();
            UserEntry = new HashSet<UserEntry>();
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public int? LikesQuantity { get; set; }
        public string Image { get; set; }

        public virtual ICollection<EventEntry> EventEntry { get; set; }
        public virtual ICollection<UserEntry> UserEntry { get; set; }
    }
}
