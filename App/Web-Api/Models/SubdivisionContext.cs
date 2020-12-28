using Domain.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_Api.Models
{
    public class SubdivisionContext:DbContext
    {
        public DbSet<Subdivision> subdivisions { get; set; }
        public DbSet<Location> locations { get; set; }
        public DbSet<ActualData> actualDataForDate { get; set; }

    }
}
