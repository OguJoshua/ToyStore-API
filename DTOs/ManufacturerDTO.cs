using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToyStore_API.DTOs
{
    public class ManufacturerDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string Profile { get; set; }

        public virtual IList<ToyDTO> Toys { get; set; }
    }

    public class ManufacturerCreateDTO
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string Profile { get; set; }

    }
    public class ManufacturerUpdateDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }


        public string Profile { get; set; }

        
    }

}
