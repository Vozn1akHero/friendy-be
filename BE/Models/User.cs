﻿using System;
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
            ChatParticipants = new HashSet<ChatParticipants>();
            ChatSecondParticipant = new HashSet<Chat>();
            Comment = new HashSet<Comment>();
            Event = new HashSet<Event>();
            EventAdmins = new HashSet<EventAdmins>();
            EventParticipants = new HashSet<EventParticipants>();
            Friend = new HashSet<Friend>();
            FriendRequest = new HashSet<FriendRequest>();
            PostLike = new HashSet<PostLike>();
            Session = new HashSet<Session>();
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
        public int? AdditionalInfoId { get; set; }
        public int? AuthenticationSessionId { get; set; }

        public virtual UserAdditionalInfo AdditionalInfo { get; set; }
        public virtual AuthenticationSession AuthenticationSession { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual ICollection<Chat> ChatFirstParticipant { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageReceiver { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageUser { get; set; }
        public virtual ICollection<ChatParticipants> ChatParticipants { get; set; }
        public virtual ICollection<Chat> ChatSecondParticipant { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<EventAdmins> EventAdmins { get; set; }
        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
        public virtual ICollection<Friend> Friend { get; set; }
        public virtual ICollection<FriendRequest> FriendRequest { get; set; }
        public virtual ICollection<PostLike> PostLike { get; set; }
        public virtual ICollection<Session> Session { get; set; }
        public virtual ICollection<UserEvents> UserEvents { get; set; }
        public virtual ICollection<UserFriends> UserFriends { get; set; }
        public virtual ICollection<UserImage> UserImage { get; set; }
        public virtual ICollection<UserPost> UserPost { get; set; }
    }
}
