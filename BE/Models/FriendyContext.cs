using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BE.Models
{
    public partial class FriendyContext : DbContext
    {
        public FriendyContext()
        {
        }

        public FriendyContext(DbContextOptions<FriendyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AlcoholAttitude> AlcoholAttitude { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<ChatMessage> ChatMessage { get; set; }
        public virtual DbSet<ChatMessages> ChatMessages { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<CommentLike> CommentLike { get; set; }
        public virtual DbSet<CommentResponseLike> CommentResponseLike { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Education> Education { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventAdmins> EventAdmins { get; set; }
        public virtual DbSet<EventBannedUsers> EventBannedUsers { get; set; }
        public virtual DbSet<EventImage> EventImage { get; set; }
        public virtual DbSet<EventParticipants> EventParticipants { get; set; }
        public virtual DbSet<EventParticipationRequest> EventParticipationRequest { get; set; }
        public virtual DbSet<EventPost> EventPost { get; set; }
        public virtual DbSet<FriendRequest> FriendRequest { get; set; }
        public virtual DbSet<FriendshipRecommendation> FriendshipRecommendation { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Interest> Interest { get; set; }
        public virtual DbSet<MainComment> MainComment { get; set; }
        public virtual DbSet<MaritalStatus> MaritalStatus { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<PostLike> PostLike { get; set; }
        public virtual DbSet<Religion> Religion { get; set; }
        public virtual DbSet<ResponseToComment> ResponseToComment { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<SmokingAttitude> SmokingAttitude { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAdditionalInfo> UserAdditionalInfo { get; set; }
        public virtual DbSet<UserEvents> UserEvents { get; set; }
        public virtual DbSet<UserFriendship> UserFriendship { get; set; }
        public virtual DbSet<UserImage> UserImage { get; set; }
        public virtual DbSet<UserInterests> UserInterests { get; set; }
        public virtual DbSet<UserPost> UserPost { get; set; }
        public virtual DbSet<Voivodeship> Voivodeship { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=friendy;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AlcoholAttitude>(entity =>
            {
                entity.ToTable("alcohol_attitude", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("chat", "chat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FirstParticipantId).HasColumnName("first_participant_id");

                entity.Property(e => e.SecondParticipantId).HasColumnName("second_participant_id");

                entity.HasOne(d => d.FirstParticipant)
                    .WithMany(p => p.ChatFirstParticipant)
                    .HasForeignKey(d => d.FirstParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_chat_user");

                entity.HasOne(d => d.SecondParticipant)
                    .WithMany(p => p.ChatSecondParticipant)
                    .HasForeignKey(d => d.SecondParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_chat_user1");
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("chat_message", "chat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.ImagePath)
                    .HasColumnName("image_path")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.ChatMessageReceiver)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_chat_message_user1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChatMessageUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_chat_message_user");
            });

            modelBuilder.Entity<ChatMessages>(entity =>
            {
                entity.ToTable("chat_messages", "chat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChatId).HasColumnName("chat_id");

                entity.Property(e => e.MessageId).HasColumnName("message_id");

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.ChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_chat_messages_chat");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_chat_messages_chat_message");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city", "geo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lon).HasColumnName("lon");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.VoivodeshipId).HasColumnName("voivodeship_id");

                entity.HasOne(d => d.Voivodeship)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.VoivodeshipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_city_voivodeship");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment", "comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_comment_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_user");
            });

            modelBuilder.Entity<CommentLike>(entity =>
            {
                entity.ToTable("comment_like", "comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.CommentLike)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_like_comment");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentLike)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_like_user");
            });

            modelBuilder.Entity<CommentResponseLike>(entity =>
            {
                entity.ToTable("comment_response_like", "comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentResponseId).HasColumnName("comment_response_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.CommentResponse)
                    .WithMany(p => p.CommentResponseLike)
                    .HasForeignKey(d => d.CommentResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_response_like_comment");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentResponseLike)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_response_like_user");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("district", "geo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lon).HasColumnName("lon");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.District)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_district_city");
            });

            modelBuilder.Entity<Education>(entity =>
            {
                entity.ToTable("education", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event", "event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar)
                    .IsRequired()
                    .HasColumnName("avatar")
                    .IsUnicode(false);

                entity.Property(e => e.Background)
                    .HasColumnName("background")
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreatorId).HasColumnName("creator_id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.EntryPrice)
                    .HasColumnName("entry_price")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ParticipantsAmount).HasColumnName("participants_amount");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.StreetNumber)
                    .IsRequired()
                    .HasColumnName("street_number")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_user");
            });

            modelBuilder.Entity<EventAdmins>(entity =>
            {
                entity.ToTable("event_admins", "event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventAdmins)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_admins_event");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventAdmins)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_admin_user");
            });

            modelBuilder.Entity<EventBannedUsers>(entity =>
            {
                entity.ToTable("event_banned_users", "event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventBannedUsers)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_banned_users_event");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventBannedUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_banned_users_user");
            });

            modelBuilder.Entity<EventImage>(entity =>
            {
                entity.ToTable("event_image", "event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventImage)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_photo_event");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.EventImage)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_image_image");
            });

            modelBuilder.Entity<EventParticipants>(entity =>
            {
                entity.ToTable("event_participants", "event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.ParticipantId).HasColumnName("participant_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventParticipants)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_participants_event");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.EventParticipants)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_participants_user");
            });

            modelBuilder.Entity<EventParticipationRequest>(entity =>
            {
                entity.ToTable("event_participation_request", "event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.IssuerId).HasColumnName("issuer_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventParticipationRequest)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_participation_request_event");

                entity.HasOne(d => d.Issuer)
                    .WithMany(p => p.EventParticipationRequest)
                    .HasForeignKey(d => d.IssuerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_participation_request_user");
            });

            modelBuilder.Entity<EventPost>(entity =>
            {
                entity.ToTable("event_post", "event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventPost)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_entry_event");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.EventPost)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_event_post_post");
            });

            modelBuilder.Entity<FriendRequest>(entity =>
            {
                entity.ToTable("friend_request", "friendship");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.FriendRequestAuthor)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_friend_request_author");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.FriendRequestReceiver)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_friend_request_receiver");
            });

            modelBuilder.Entity<FriendshipRecommendation>(entity =>
            {
                entity.ToTable("friendship_recommendation", "friendship");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IssuerId).HasColumnName("issuer_id");

                entity.Property(e => e.LastCheckupTime)
                    .HasColumnName("last_checkup_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.PotentialFriendId).HasColumnName("potential_friend_id");

                entity.HasOne(d => d.Issuer)
                    .WithMany(p => p.FriendshipRecommendationIssuer)
                    .HasForeignKey(d => d.IssuerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_friendship_recommendation_issuer");

                entity.HasOne(d => d.PotentialFriend)
                    .WithMany(p => p.FriendshipRecommendationPotentialFriend)
                    .HasForeignKey(d => d.PotentialFriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_friendship_recommendation_potential_friend");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("gender", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image", "image");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnName("path")
                    .IsUnicode(false);

                entity.Property(e => e.PublishDate).HasColumnName("publish_date");
            });

            modelBuilder.Entity<Interest>(entity =>
            {
                entity.ToTable("interest", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MainComment>(entity =>
            {
                entity.ToTable("main_comment", "comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.MainComment)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_main_comment_comment");
            });

            modelBuilder.Entity<MaritalStatus>(entity =>
            {
                entity.ToTable("marital_status", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post", "post");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ImagePath)
                    .HasColumnName("image_path")
                    .HasMaxLength(1500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.ToTable("post_like", "post");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostLike)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_post_like_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostLike)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_post_like_user");
            });

            modelBuilder.Entity<Religion>(entity =>
            {
                entity.ToTable("religion", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ResponseToComment>(entity =>
            {
                entity.ToTable("response_to_comment", "comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.MainCommentId).HasColumnName("main_comment_id");

                entity.Property(e => e.ResponseToCommentId).HasColumnName("response_to_comment_id");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.ResponseToCommentComment)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_response_to_comment_content");

                entity.HasOne(d => d.MainComment)
                    .WithMany(p => p.ResponseToComment)
                    .HasForeignKey(d => d.MainCommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_response_to_comment_main_comment");

                entity.HasOne(d => d.ResponseToCommentNavigation)
                    .WithMany(p => p.ResponseToCommentResponseToCommentNavigation)
                    .HasForeignKey(d => d.ResponseToCommentId)
                    .HasConstraintName("FK_response_to_comment_comment");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("session");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ConnectionEnd).HasColumnName("connection_end");

                entity.Property(e => e.ConnectionId)
                    .HasColumnName("connection_id")
                    .IsUnicode(false);

                entity.Property(e => e.ConnectionStart).HasColumnName("connection_start");
            });

            modelBuilder.Entity<SmokingAttitude>(entity =>
            {
                entity.ToTable("smoking_attitude", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdditionalInfoId).HasColumnName("additional_info_id");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .IsUnicode(false)
                    .HasDefaultValueSql("('wwwroot/UserAvatar/default-user-avatar.png')");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.EducationId).HasColumnName("education_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.GenderId).HasColumnName("gender_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileBg)
                    .HasColumnName("profile_bg")
                    .IsUnicode(false);

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("surname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AdditionalInfo)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.AdditionalInfoId)
                    .HasConstraintName("FK_user_user_additional_info");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_city");

                entity.HasOne(d => d.Education)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.EducationId)
                    .HasConstraintName("FK_user_education");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_gender");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK_user_session");
            });

            modelBuilder.Entity<UserAdditionalInfo>(entity =>
            {
                entity.ToTable("user_additional_info", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AlcoholAttitudeId).HasColumnName("alcohol_attitude_id");

                entity.Property(e => e.MaritalStatusId).HasColumnName("marital_status_id");

                entity.Property(e => e.ReligionId).HasColumnName("religion_id");

                entity.Property(e => e.SmokingAttitudeId).HasColumnName("smoking_attitude_id");

                entity.HasOne(d => d.AlcoholAttitude)
                    .WithMany(p => p.UserAdditionalInfo)
                    .HasForeignKey(d => d.AlcoholAttitudeId)
                    .HasConstraintName("FK_user_additional_info_alcohol_attitude");

                entity.HasOne(d => d.MaritalStatus)
                    .WithMany(p => p.UserAdditionalInfo)
                    .HasForeignKey(d => d.MaritalStatusId)
                    .HasConstraintName("FK_user_additional_info_family_status");

                entity.HasOne(d => d.Religion)
                    .WithMany(p => p.UserAdditionalInfo)
                    .HasForeignKey(d => d.ReligionId)
                    .HasConstraintName("FK_user_additional_info_religion");

                entity.HasOne(d => d.SmokingAttitude)
                    .WithMany(p => p.UserAdditionalInfo)
                    .HasForeignKey(d => d.SmokingAttitudeId)
                    .HasConstraintName("FK_user_additional_info_smoking_attitude");
            });

            modelBuilder.Entity<UserEvents>(entity =>
            {
                entity.ToTable("user_events", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.UserEvents)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_events_event");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserEvents)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_events_user");
            });

            modelBuilder.Entity<UserFriendship>(entity =>
            {
                entity.ToTable("user_friendship", "friendship");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FirstFriendId).HasColumnName("first_friend_id");

                entity.Property(e => e.SecondFriendId).HasColumnName("second_friend_id");

                entity.HasOne(d => d.FirstFriend)
                    .WithMany(p => p.UserFriendshipFirstFriend)
                    .HasForeignKey(d => d.FirstFriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_friends_user");

                entity.HasOne(d => d.SecondFriend)
                    .WithMany(p => p.UserFriendshipSecondFriend)
                    .HasForeignKey(d => d.SecondFriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_friends_user1");
            });

            modelBuilder.Entity<UserImage>(entity =>
            {
                entity.ToTable("user_image", "user_info");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.UserImage)
                    .HasForeignKey<UserImage>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_image_image");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserImage)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_image_user");
            });

            modelBuilder.Entity<UserInterests>(entity =>
            {
                entity.ToTable("user_interests", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InterestId).HasColumnName("interest_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Wage).HasColumnName("wage");

                entity.HasOne(d => d.Interest)
                    .WithMany(p => p.UserInterests)
                    .HasForeignKey(d => d.InterestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_interests_interest");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserInterests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_interests_user");
            });

            modelBuilder.Entity<UserPost>(entity =>
            {
                entity.ToTable("user_post", "user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.UserPost)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_user_post_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPost)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_entry_user");
            });

            modelBuilder.Entity<Voivodeship>(entity =>
            {
                entity.ToTable("voivodeship", "geo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });
        }
    }
}
