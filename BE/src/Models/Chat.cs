using System.Collections.Generic;

namespace BE.Models
{
    public class Chat
    {
        public Chat()
        {
            ChatMessages = new HashSet<ChatMessages>();
        }

        public int Id { get; set; }
        public int FirstParticipantId { get; set; }
        public int SecondParticipantId { get; set; }

        public virtual User FirstParticipant { get; set; }
        public virtual User SecondParticipant { get; set; }
        public virtual ICollection<ChatMessages> ChatMessages { get; set; }
    }
}