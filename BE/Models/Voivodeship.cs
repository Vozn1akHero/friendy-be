using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class Voivodeship
    {
        public Voivodeship()
        {
            City = new HashSet<City>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}
