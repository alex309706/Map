using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core
{
    public class ActualData
    {
        public DateTime date { get; set; }
        IEnumerable<Subdivision> subdivisions { get; set; }
    }
}
