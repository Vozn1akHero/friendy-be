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
            CommentLike = new HashSet<CommentLike>();
            Event = new HashSet<Event>();
            EventAdmins = new HashSet<EventAdmins>();
            EventBannedUsers = new HashSet<EventBannedUsers>();
            EventParticipants = new HashSet<EventParticipants>();
            EventParticipationRequest = new HashSet<EventParticipationRequest>();
            FriendRequestAuthor = new HashSet<FriendRequest>();
            FriendRequestReceiver = new HashSet<FriendRequest>();
            FriendshipRecommendationIssuer = new HashSet<FriendshipRecommendation>();
            FriendshipRecommendationPotentialFriend = new HashSet<FriendshipRecommendation>();
            PostLike = new HashSet<PostLike>();
            Session = new HashSet<Session>();
            UserEvents = new HashSet<UserEvents>();
            UserFriendshipFirstFriend = new HashSet<UserFriendship>();
            UserFriendshipSecondFriend = new HashSet<UserFriendship>();
            UserImage = new HashSet<UserImage>();
            UserInterests = new HashSet<UserInterests>();
            UserPost = new HashSet<UserPost>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public int GenderId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public string ProfileBg { get; set; }
        public string Status { get; set; }
        public int? EducationId { get; set; }
        public int? AdditionalInfoId { get; set; }
        public int? SessionId { get; set; }

        public virtual UserAdditionalInfo AdditionalInfo { get; set; }
        public virtual Education Education { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Session SessionNavigation { get; set; }
        public virtual ICollection<Chat> ChatFirstParticipant { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageReceiver { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageUser { get; set; }
        public virtual ICollection<Chat> ChatSecondParticipant { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<CommentLike> CommentLike { get; set; }
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<EventAdmins> EventAdmins { get; set; }
        public virtual ICollection<EventBannedUsers> EventBannedUsers { get; set; }
        public virtual ICollection<EventParticipants> EventParticipants { get; set; }
        public virtual ICollection<EventParticipationRequest> EventParticipationRequest { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestAuthor { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestReceiver { get; set; }
        public virtual ICollection<FriendshipRecommendation> FriendshipRecommendationIssuer { get; set; }
        public virtual ICollection<FriendshipRecommendation> FriendshipRecommendationPotentialFriend { get; set; }
        public virtual ICollection<PostLike> PostLike { get; set; }
        public virtual ICollection<Session> Session { get; set; }
        public virtual ICollection<UserEvents> UserEvents { get; set; }
        public virtual ICollection<UserFriendship> UserFriendshipFirstFriend { get; set; }
        public virtual ICollection<UserFriendship> UserFriendshipSecondFriend { get; set; }
        public virtual ICollection<UserImage> UserImage { get; set; }
        public virtual ICollection<UserInterests> UserInterests { get; set; }
        public virtual ICollection<UserPost> UserPost { get; set; }
    }
}
