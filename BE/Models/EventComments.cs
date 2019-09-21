using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventComments
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int EventPostId { get; set; }

        public virtual EventPost EventPost { get; set; }
    }
}
