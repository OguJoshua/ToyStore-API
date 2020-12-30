using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyStore_API.Contracts;
using ToyStore_API.Data;

namespace ToyStore_API.Services
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ApplicationDbContext _db;

        public ManufacturerRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Manufacturer entity)
        {
           await _db.Manufacturers.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Manufacturer entity)
        {
            _db.Manufacturers.Remove(entity);
            return await Save();
        }

        public async Task<List<Manufacturer>> FindAll()
        {
            var manufacturers = await _db.Manufacturers.ToListAsync();
            return manufacturers;
        }

        public async Task<Manufacturer> FindById(int id)
        {
            var manufacturers = await _db.Manufacturers.FindAsync(id);
            return manufacturers;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Manufacturers.AnyAsync(q => q.Id ==id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;

        }

        public async Task<bool> Update(Manufacturer entity)
        {
            _db.Manufacturers.Update(entity);
            return await Save();
        }
    }
}
