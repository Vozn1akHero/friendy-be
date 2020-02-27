using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserAdditionalInfo
    {
        public UserAdditionalInfo()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public int? MaritalStatusId { get; set; }
        public int? ReligionId { get; set; }
        public int? AlcoholAttitudeId { get; set; }
        public int? SmokingAttitudeId { get; set; }

        public virtual AlcoholAttitude AlcoholAttitude { get; set; }
        public virtual MaritalStatus MaritalStatus { get; set; }
        public virtual Religion Religion { get; set; }
        public virtual SmokingAttitude SmokingAttitude { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
