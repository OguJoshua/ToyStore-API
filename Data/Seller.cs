using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyStore_API.Data
{
    [Table("Sellers")]
    public partial class Seller
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Profile { get; set; }

        public virtual IList<Toy> Toys { get; set; }
    }
}
