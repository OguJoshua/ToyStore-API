using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyStore_API.Data
{
    [Table("Toys")]
    public partial class Toy
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int YearOfProduction { get; set; }
        public int ModelNumber { get; set; }

        public string Summary { get; set; }
        public string Image { get; set; }
        public double? Price { get; set; }
        public int? ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; } 

    }
}
