using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class AuthenticationSession
    {
        public AuthenticationSession()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Hash { get; set; }
        public string Token { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
