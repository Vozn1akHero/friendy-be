﻿namespace BE.Models
{
    public class UserFriendship
    {
        public int Id { get; set; }
        public int FirstFriendId { get; set; }
        public int SecondFriendId { get; set; }

        public virtual User FirstFriend { get; set; }
        public virtual User SecondFriend { get; set; }
    }
}