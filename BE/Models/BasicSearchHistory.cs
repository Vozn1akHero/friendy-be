using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class BasicSearchHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string InsertedUserName { get; set; }
        public string InsertedUserSurname { get; set; }

        public virtual User User { get; set; }
    }
}
