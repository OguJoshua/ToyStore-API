using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToyStore_API.DTOs
{
    public class ToyDTO
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int? YearOfProduction { get; set; }
        public int ModelNumber { get; set; }
       public string Summary { get; set; }
        public string Image { get; set; }
        public double? Price { get; set; }
        public string File { get; set; }
        public int? SellerId { get; set; }
       public virtual SellerDTO seller { get; set; }
    }

    public class ToyCreateDTO
    { 
       
        public int Name { get; set; }
        public int? YearOfProduction { get; set; }
        public int ModelNumber { get; set; }
        [StringLength(500)]
        public string Summary { get; set; }
        public string Image { get; set; }
        public double? Price { get; set; }
        [Required]
        public int sellerId { get; set; }
        public string File { get; set; }

    }
    public class ToyUpdateDTO
    {

        public int Id { get; set; }
        [Required]
        public int Name { get; set; }
        public int? YearOfProduction { get; set; }
        public int ModelNumber { get; set; }
        [StringLength(500)]
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public string File { get; set; }


    }
}
