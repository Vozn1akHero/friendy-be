using System.Collections.Generic;

namespace BE.Models
{
    public class SmokingAttitude
    {
        public SmokingAttitude()
        {
            UserAdditionalInfo = new HashSet<UserAdditionalInfo>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<UserAdditionalInfo> UserAdditionalInfo { get; set; }
    }
}