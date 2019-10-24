﻿using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserAdditionalInfo
    {
        public UserAdditionalInfo()
        {
            User = new HashSet<User>();
            UserInterests = new HashSet<UserInterests>();
        }

        public int Id { get; set; }
        public int? EducationId { get; set; }
        public int? FamilyStatusId { get; set; }
        public int? ReligionId { get; set; }
        public int? AlcoholAttitudeId { get; set; }
        public int? SmokingAttitudeId { get; set; }
        public int? DrugsAttitudeId { get; set; }

        public virtual AlcoholAttitude AlcoholAttitude { get; set; }
        public virtual DrugsAttitude DrugsAttitude { get; set; }
        public virtual Education Education { get; set; }
        public virtual FamilyStatus FamilyStatus { get; set; }
        public virtual Religion Religion { get; set; }
        public virtual SmokingAttitude SmokingAttitude { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<UserInterests> UserInterests { get; set; }
    }
}