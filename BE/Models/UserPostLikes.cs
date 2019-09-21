using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserPostLikes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserPostId { get; set; }

        public virtual UserPost UserPost { get; set; }
    }
}
