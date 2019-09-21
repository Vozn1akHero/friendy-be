using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserImage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }

        public virtual User User { get; set; }
    }
}
