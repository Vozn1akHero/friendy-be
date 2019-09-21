﻿using System;
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
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<DrugsAttitude> DrugsAttitude { get; set; }
        public virtual DbSet<Education> Education { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventAdmin> EventAdmin { get; set; }
        public virtual DbSet<EventComments> EventComments { get; set; }
        public virtual DbSet<EventImage> EventImage { get; set; }
        public virtual DbSet<EventPost> EventPost { get; set; }
        public virtual DbSet<EventPostLikes> EventPostLikes { get; set; }
        public virtual DbSet<FamilyStatus> FamilyStatus { get; set; }
        public virtual DbSet<Friend> Friend { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Interest> Interest { get; set; }
        public virtual DbSet<Religion> Religion { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<SmokingAttitude> SmokingAttitude { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAdditionalInfo> UserAdditionalInfo { get; set; }
        public virtual DbSet<UserEvents> UserEvents { get; set; }
        public virtual DbSet<UserFriends> UserFriends { get; set; }
        public virtual DbSet<UserImage> UserImage { get; set; }
        public virtual DbSet<UserInterests> UserInterests { get; set; }
        public virtual DbSet<UserPost> UserPost { get; set; }
        public virtual DbSet<UserPostComments> UserPostComments { get; set; }
        public virtual DbSet<UserPostLikes> UserPostLikes { get; set; }

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
                entity.ToTable("alcohol_attitude");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comment_user");
            });

            modelBuilder.Entity<DrugsAttitude>(entity =>
            {
                entity.ToTable("drugs_attitude");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Education>(entity =>
            {
                entity.ToTable("education");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Day).HasColumnName("day");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.EntryPrice)
                    .HasColumnName("entry_price")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Hour).HasColumnName("hour");

                entity.Property(e => e.Minutre).HasColumnName("minutre");

                entity.Property(e => e.Month).HasColumnName("month");

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

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<EventAdmin>(entity =>
            {
                entity.ToTable("event_admin");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventAdmin)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_event_admin_user");
            });

            modelBuilder.Entity<EventComments>(entity =>
            {
                entity.ToTable("event_comments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.EventPostId).HasColumnName("event_post_id");

                entity.HasOne(d => d.EventPost)
                    .WithMany(p => p.EventComments)
                    .HasForeignKey(d => d.EventPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_comments_comment");
            });

            modelBuilder.Entity<EventImage>(entity =>
            {
                entity.ToTable("event_image");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .IsUnicode(false);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventImage)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_photo_event");
            });

            modelBuilder.Entity<EventPost>(entity =>
            {
                entity.ToTable("event_post");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.EventId).HasColumnName("event_id");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.LikesQuantity).HasColumnName("likes_quantity");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventPost)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_entry_event");
            });

            modelBuilder.Entity<EventPostLikes>(entity =>
            {
                entity.ToTable("event_post_likes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EventPostId).HasColumnName("event_post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventPostLikes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_event_post_likes_user");
            });

            modelBuilder.Entity<FamilyStatus>(entity =>
            {
                entity.ToTable("family_status");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.ToTable("friend");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FriendId).HasColumnName("friend_id");

                entity.HasOne(d => d.FriendNavigation)
                    .WithMany(p => p.Friend)
                    .HasForeignKey(d => d.FriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_friend_user");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("gender");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Interest>(entity =>
            {
                entity.ToTable("interest");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Religion>(entity =>
            {
                entity.ToTable("religion");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("session");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasColumnName("hash")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnName("token")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SmokingAttitude>(entity =>
            {
                entity.ToTable("smoking_attitude");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdditionalInfoId).HasColumnName("additional_info_id");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .IsUnicode(false);

                entity.Property(e => e.BirthMonth).HasColumnName("birth_month");

                entity.Property(e => e.BirthYear).HasColumnName("birth_year");

                entity.Property(e => e.Birthday).HasColumnName("birthday");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
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
                    .HasMaxLength(8000)
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

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_sex");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK_user_session");
            });

            modelBuilder.Entity<UserAdditionalInfo>(entity =>
            {
                entity.ToTable("user_additional_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AlcoholAttitudeId).HasColumnName("alcohol_attitude_id");

                entity.Property(e => e.DrugsAttitudeId).HasColumnName("drugs_attitude_id");

                entity.Property(e => e.EducationId).HasColumnName("education_id");

                entity.Property(e => e.FamilyStatusId).HasColumnName("family_status_id");

                entity.Property(e => e.ReligionId).HasColumnName("religion_id");

                entity.Property(e => e.SmokingAttitudeId).HasColumnName("smoking_attitude_id");

                entity.HasOne(d => d.AlcoholAttitude)
                    .WithMany(p => p.UserAdditionalInfo)
                    .HasForeignKey(d => d.AlcoholAttitudeId)
                    .HasConstraintName("FK_user_additional_info_alcohol_attitude");

                entity.HasOne(d => d.DrugsAttitude)
                    .WithMany(p => p.UserAdditionalInfo)
                    .HasForeignKey(d => d.DrugsAttitudeId)
                    .HasConstraintName("FK_user_additional_info_drugs_attitude");

                entity.HasOne(d => d.Education)
                    .WithMany(p => p.UserAdditionalInfo)
                    .HasForeignKey(d => d.EducationId)
                    .HasConstraintName("FK_user_additional_info_education");

                entity.HasOne(d => d.FamilyStatus)
                    .WithMany(p => p.UserAdditionalInfo)
                    .HasForeignKey(d => d.FamilyStatusId)
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
                entity.ToTable("user_events");

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

            modelBuilder.Entity<UserFriends>(entity =>
            {
                entity.ToTable("user_friends");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FriendId).HasColumnName("friend_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Friend)
                    .WithMany(p => p.UserFriends)
                    .HasForeignKey(d => d.FriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_friends_friend");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFriends)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_friends_user");
            });

            modelBuilder.Entity<UserImage>(entity =>
            {
                entity.ToTable("user_image");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserImage)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_image_user");
            });

            modelBuilder.Entity<UserInterests>(entity =>
            {
                entity.ToTable("user_interests");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdditionalInfoId).HasColumnName("additional_info_id");

                entity.Property(e => e.InterestId).HasColumnName("interest_id");

                entity.HasOne(d => d.AdditionalInfo)
                    .WithMany(p => p.UserInterests)
                    .HasForeignKey(d => d.AdditionalInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_interests_user_additional_info");

                entity.HasOne(d => d.Interest)
                    .WithMany(p => p.UserInterests)
                    .HasForeignKey(d => d.InterestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_interests_interest");
            });

            modelBuilder.Entity<UserPost>(entity =>
            {
                entity.ToTable("user_post");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPost)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_entry_user");
            });

            modelBuilder.Entity<UserPostComments>(entity =>
            {
                entity.ToTable("user_post_comments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.UserPostId).HasColumnName("user_post_id");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.UserPostComments)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_entry_comments_comment");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.UserPostComments)
                    .HasForeignKey(d => d.UserPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_entry_comments_user_entry");
            });

            modelBuilder.Entity<UserPostLikes>(entity =>
            {
                entity.ToTable("user_post_likes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserPostId).HasColumnName("user_post_id");

                entity.HasOne(d => d.UserPost)
                    .WithMany(p => p.UserPostLikes)
                    .HasForeignKey(d => d.UserPostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_post_likes_user_post");
            });
        }
    }
}
