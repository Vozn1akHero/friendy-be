using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class User
    {
        public User()
        {
            Comment = new HashSet<Comment>();
            EventAdmin = new HashSet<EventAdmin>();
            Friend = new HashSet<Friend>();
            UserEntry = new HashSet<UserEntry>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int GenderId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int BirthYear { get; set; }
        public int Birthday { get; set; }
        public int BirthMonth { get; set; }
        public string Avatar { get; set; }
        public string ProfileBg { get; set; }
        public string Status { get; set; }
        public int? SessionId { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Session Session { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<EventAdmin> EventAdmin { get; set; }
        public virtual ICollection<Friend> Friend { get; set; }
        public virtual ICollection<UserEntry> UserEntry { get; set; }
    }
}
