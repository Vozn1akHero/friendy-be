using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Friend
    {
        public Friend()
        {
            UserFriends = new HashSet<UserFriends>();
        }

        public int Id { get; set; }
        public int FriendId { get; set; }

        public virtual User FriendNavigation { get; set; }
        public virtual ICollection<UserFriends> UserFriends { get; set; }
    }
}
