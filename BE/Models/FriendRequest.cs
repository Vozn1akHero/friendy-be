using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class FriendRequest
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int ReceiverId { get; set; }

        public virtual User Author { get; set; }
        public virtual Friend Receiver { get; set; }
    }
}
