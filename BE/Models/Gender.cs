using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Gender
    {
        public Gender()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
