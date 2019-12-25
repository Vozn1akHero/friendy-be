using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class RecVisitedProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VisitedUserProfileId { get; set; }

        public virtual User User { get; set; }
        public virtual User VisitedUserProfile { get; set; }
    }
}
