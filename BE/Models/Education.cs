using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Education
    {
        public Education()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
