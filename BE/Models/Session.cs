using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ConnectionStart { get; set; }
        public DateTime? ConnectionEnd { get; set; }

        public virtual User User { get; set; }
    }
}
