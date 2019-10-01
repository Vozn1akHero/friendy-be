using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Chat
    {
        public Chat()
        {
            ChatParticipants = new HashSet<ChatParticipants>();
        }

        public int Id { get; set; }
        public string UrlHash { get; set; }

        public virtual ICollection<ChatParticipants> ChatParticipants { get; set; }
    }
}
