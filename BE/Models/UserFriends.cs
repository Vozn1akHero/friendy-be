using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserFriends
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }

        public virtual Friend Friend { get; set; }
        public virtual User User { get; set; }
    }
}
