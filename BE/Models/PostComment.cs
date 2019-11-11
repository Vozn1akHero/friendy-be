using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class PostComment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
