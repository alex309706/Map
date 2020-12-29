using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Core
{
    public class Location
    {
        [Key]
        public string  name { get; set; }
        public Coordinates coordinates { get; set; }
    }
}
