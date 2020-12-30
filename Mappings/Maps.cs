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
            CreateMap<Manufacturer, ManufacturerDTO>().ReverseMap();
            CreateMap<Manufacturer, ManufacturerCreateDTO>().ReverseMap();
            CreateMap<Manufacturer, ManufacturerUpdateDTO>().ReverseMap();
            CreateMap<Toy, ToyDTO>().ReverseMap();
            CreateMap<Toy, ToyDTO>().ReverseMap();
        }
        
    }
}
