using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserEntry
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public int UserId { get; set; }

        public virtual Entry Entry { get; set; }
        public virtual User User { get; set; }
    }
}
