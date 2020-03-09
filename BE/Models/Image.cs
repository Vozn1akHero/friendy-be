using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Image
    {
        public Image()
        {
            EventImage = new HashSet<EventImage>();
            UserImage = new HashSet<UserImage>();
        }

        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime PublishDate { get; set; }

        public virtual ICollection<EventImage> EventImage { get; set; }
        public virtual ICollection<UserImage> UserImage { get; set; }
    }
}
