using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Friend
    {
        public int Id { get; set; }
        public int FriendId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
