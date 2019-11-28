using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class User
    {
        public User()
        {
            ChatFirstParticipant = new HashSet<Chat>();
            ChatMessageReceiver = new HashSet<ChatMessage>();
            ChatMessageUser = new HashSet<ChatMessage>();
            ChatSecondParticipant = new HashSet<Chat>();
            Comment = new HashSet<Comment>();
            Event = new HashSet<Event>();
            EventAdmins = new HashSet<EventAdmins>();
            EventParticipants = new HashSet<EventParticipants>();
            FriendRequestAuthor = new HashSet<FriendRequest>();
            FriendRequestReceiver = new HashSet<FriendRequest>();
            PostLike = new HashSet<PostLike>();
            Session = new HashSet<Session>();
            UserEvents = new HashSet<UserEvents>();
            UserFriendshipFirstFriend = new HashSet<UserFriendship>();
            UserFriendshipSecondFriend = new HashSet<UserFriendship>();
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
        public int? AdditionalInfoId { get; set; }
        public int? AuthenticationSessionId { get; set; }
        public int? SessionId { get; set; }

        public virtual UserAdditionalInfo AdditionalInfo { get; set; }
        public virtual AuthenticationSession AuthenticationSession { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Session SessionNavigation { get; set; }
        public virtual ICollection<Chat> ChatFirstParticipant { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageReceiver { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageUser { get; set; }
        public virtual ICollection<Chat> ChatSecondParticipant { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<EventAdmins> EventAdmins { get; set; }
        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestAuthor { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestReceiver { get; set; }
        public virtual ICollection<PostLike> PostLike { get; set; }
        public virtual ICollection<Session> Session { get; set; }
        public virtual ICollection<UserEvents> UserEvents { get; set; }
        public virtual ICollection<UserFriendship> UserFriendshipFirstFriend { get; set; }
        public virtual ICollection<UserFriendship> UserFriendshipSecondFriend { get; set; }
        public virtual ICollection<UserImage> UserImage { get; set; }
        public virtual ICollection<UserPost> UserPost { get; set; }
    }
}
