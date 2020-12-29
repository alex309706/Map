using System;
using System.Collections.Generic;

#nullable disable

namespace WebApiSubdivisions.Models
{
    public partial class Subdivision
    {
        public Subdivision()
        {
            ActualData = new HashSet<ActualDatum>();
        }

        public int IdSubdivision { get; set; }
        public string TypeOfSubdivision { get; set; }
        public string Name { get; set; }
        public string Composition { get; set; }
        public int? Strength { get; set; }
        public string Document { get; set; }
        public int? Commander { get; set; }

        public virtual Commander CommanderNavigation { get; set; }
        public virtual ICollection<ActualDatum> ActualData { get; set; }
    }
}
