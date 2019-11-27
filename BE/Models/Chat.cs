using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Chat
    {
        public Chat()
        {
            ChatMessages = new HashSet<ChatMessages>();
            ChatParticipants = new HashSet<ChatParticipants>();
        }

        public int Id { get; set; }
        public int FirstParticipantId { get; set; }
        public int SecondParticipantId { get; set; }

        public virtual User FirstParticipant { get; set; }
        public virtual User SecondParticipant { get; set; }
        public virtual ICollection<ChatMessages> ChatMessages { get; set; }
        public virtual ICollection<ChatParticipants> ChatParticipants { get; set; }
    }
}
