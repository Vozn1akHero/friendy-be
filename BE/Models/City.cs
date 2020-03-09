using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class City
    {
        public City()
        {
            District = new HashSet<District>();
            Event = new HashSet<Event>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public int VoivodeshipId { get; set; }
        public string Title { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }

        public virtual Voivodeship Voivodeship { get; set; }
        public virtual ICollection<District> District { get; set; }
        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
