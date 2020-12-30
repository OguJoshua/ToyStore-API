using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToyStore_API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Toy> Toys { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }

}

