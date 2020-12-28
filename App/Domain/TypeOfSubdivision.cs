using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace Domain.Core
{
    public class TypeOfSubdivision
    {
        [Key]
        public string Name{ get; set; }
    }
}
