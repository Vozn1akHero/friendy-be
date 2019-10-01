using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class User
    {
        public User()
        {
            ChatMessage = new HashSet<ChatMessage>();
            ChatParticipants = new HashSet<ChatParticipants>();
            Comment = new HashSet<Comment>();
            EventAdmins = new HashSet<EventAdmins>();
            EventParticipants = new HashSet<EventParticipants>();
            EventPostLikes = new HashSet<EventPostLikes>();
            Friend = new HashSet<Friend>();
            UserEvents = new HashSet<UserEvents>();
            UserFriends = new HashSet<UserFriends>();
            UserImage = new HashSet<UserImage>();
            UserPost = new HashSet<UserPost>();
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
        public int? AdditionalInfoId { get; set; }

        public virtual UserAdditionalInfo AdditionalInfo { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Session Session { get; set; }
        public virtual ICollection<ChatMessage> ChatMessage { get; set; }
        public virtual ICollection<ChatParticipants> ChatParticipants { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<EventAdmins> EventAdmins { get; set; }
        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
        public virtual ICollection<EventPostLikes> EventPostLikes { get; set; }
        public virtual ICollection<Friend> Friend { get; set; }
        public virtual ICollection<UserEvents> UserEvents { get; set; }
        public virtual ICollection<UserFriends> UserFriends { get; set; }
        public virtual ICollection<UserImage> UserImage { get; set; }
        public virtual ICollection<UserPost> UserPost { get; set; }
    }
}
