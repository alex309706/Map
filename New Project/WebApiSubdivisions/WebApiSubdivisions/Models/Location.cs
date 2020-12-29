using System;
using System.Collections.Generic;

#nullable disable

namespace WebApiSubdivisions.Models
{
    public partial class Location
    {
        public Location()
        {
            ActualData = new HashSet<ActualDatum>();
        }

        public string Name { get; set; }
        public decimal? X { get; set; }
        public decimal? Y { get; set; }

        public virtual ICollection<ActualDatum> ActualData { get; set; }
    }
}
