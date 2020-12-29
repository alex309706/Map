using System;
using System.Collections.Generic;

#nullable disable

namespace WebApiSubdivisions.Models
{
    public partial class ActualDatum
    {
        public DateTime? DateTime { get; set; }
        public int? IdSubdivision { get; set; }
        public string Location { get; set; }
        public int Id { get; set; }

        public virtual Subdivision IdSubdivisionNavigation { get; set; }
        public virtual Location LocationNavigation { get; set; }
    }
}
