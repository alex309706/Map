using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core
{
    //подразделение
    public class Subdivision
    {
        public string Name{ get; set; }
        public string Strength { get; set; }

        public string Composition { get; set; }

        public string Document{ get; set; }
        public string Location { get; set; }

        
        public Coordinates coord { get; set; }
        
        public string Commander { get; set; }
        public string type { get; set; }

        public Subdivision(double x, double y)
        {
            coord = new Coordinates(x,y);
          
        }
    }
}
