using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class District
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Title { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }

        public virtual City City { get; set; }
    }
}
