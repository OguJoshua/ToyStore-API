using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyStore_API.Data;
using ToyStore_API.DTOs;

namespace ToyStore_API.Mappings
{
    public class Maps: Profile
    {
        public Maps()
        {
            CreateMap<Seller, SellerDTO>().ReverseMap();
            CreateMap<Seller, SellerCreateDTO>().ReverseMap();
            CreateMap<Seller, SellerUpdateDTO>().ReverseMap();
            CreateMap<Toy, ToyDTO>().ReverseMap();
            CreateMap<Toy, ToyDTO>().ReverseMap();
        }
        
    }
}
