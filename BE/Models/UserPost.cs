using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserPost
    {
        public UserPost()
        {
            UserPostComments = new HashSet<UserPostComments>();
            UserPostLikes = new HashSet<UserPostLikes>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<UserPostComments> UserPostComments { get; set; }
        public virtual ICollection<UserPostLikes> UserPostLikes { get; set; }
    }
}
