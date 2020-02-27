using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Image
    {
        public Image()
        {
            EventImage = new HashSet<EventImage>();
        }

        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime PublishDate { get; set; }

        public virtual UserImage UserImage { get; set; }
        public virtual ICollection<EventImage> EventImage { get; set; }
    }
}
