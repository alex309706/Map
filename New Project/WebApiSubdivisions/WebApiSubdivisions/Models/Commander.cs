using System;
using System.Collections.Generic;

#nullable disable

namespace WebApiSubdivisions.Models
{
    public partial class Commander
    {
        public Commander()
        {
            Subdivisions = new HashSet<Subdivision>();
        }

        public int IdCommander { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Subdivision> Subdivisions { get; set; }
    }
}
