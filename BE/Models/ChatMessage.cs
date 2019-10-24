﻿using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class ChatMessage
    {
        public ChatMessage()
        {
            ChatMessages = new HashSet<ChatMessages>();
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<ChatMessages> ChatMessages { get; set; }
    }
}