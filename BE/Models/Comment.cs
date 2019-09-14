using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int LikesQuantity { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
