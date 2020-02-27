using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class ChatMessages
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int MessageId { get; set; }

        public virtual Chat Chat { get; set; }
        public virtual ChatMessage Message { get; set; }
    }
}
