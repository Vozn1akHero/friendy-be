using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class UserImage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ImageId { get; set; }

        public virtual Image Image { get; set; }
        public virtual User User { get; set; }
    }
}
