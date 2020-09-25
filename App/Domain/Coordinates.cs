using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Core
{
    //координаты
   public class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Coordinates(double x,double y)
        {
            X = x;
            Y = y;
        }
    }
}
