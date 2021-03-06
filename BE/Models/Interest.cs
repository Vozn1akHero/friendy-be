﻿using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Interest
    {
        public Interest()
        {
            UserInterests = new HashSet<UserInterests>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<UserInterests> UserInterests { get; set; }
    }
}
