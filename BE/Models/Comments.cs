using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Comments
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public int CommentId { get; set; }
    }
}
