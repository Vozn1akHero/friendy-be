using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserInterests
    {
        public int Id { get; set; }
        public int AdditionalInfoId { get; set; }
        public int InterestId { get; set; }

        public virtual UserAdditionalInfo AdditionalInfo { get; set; }
        public virtual Interest Interest { get; set; }
    }
}
