using System;
using System.Collections.Generic;

namespace BE.Models
{
    public class Session
    {
        public Session()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public DateTime ConnectionStart { get; set; }
        public DateTime? ConnectionEnd { get; set; }
        public string ConnectionId { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}