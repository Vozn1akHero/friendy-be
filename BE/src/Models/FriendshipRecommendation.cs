using System;

namespace BE.Models
{
    public class FriendshipRecommendation
    {
        public int Id { get; set; }
        public int IssuerId { get; set; }
        public int PotentialFriendId { get; set; }
        public DateTime LastCheckupTime { get; set; }

        public virtual User Issuer { get; set; }
        public virtual User PotentialFriend { get; set; }
    }
}