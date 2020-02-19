using System;
using System.Collections.Generic;

namespace BE.Models
{
    public class Post
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
            EventPost = new HashSet<EventPost>();
            PostLike = new HashSet<PostLike>();
            UserPost = new HashSet<UserPost>();
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<EventPost> EventPost { get; set; }
        public virtual ICollection<PostLike> PostLike { get; set; }
        public virtual ICollection<UserPost> UserPost { get; set; }
    }
}