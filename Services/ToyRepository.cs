using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyStore_API.Contracts;
using ToyStore_API.Data;

namespace ToyStore_API.Services
{
    public class ToyRepository : IToyRepository
    {
        private readonly ApplicationDbContext _db;
        public ToyRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Toy entity)
        {
            await _db.Toys.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Toy entity)
        {
            _db.Toys.Remove(entity);
            return await Save();
        }

        public async Task<List<Toy>> FindAll()
        {
            var toys = await _db.Toys.ToListAsync();
            return toys; 
        }

        public async  Task<Toy> FindById(int id)
        {
            var toy = await _db.Toys.FindAsync(id);
            return toy;
        }

        public async Task<bool> isExists(int id)
        {
            var isExists = await _db.Toys.AnyAsync(q => q.Id == id);
            return isExists;
            
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async  Task<bool> Update(Toy entity)
        {
            _db.Toys.Update(entity);
            return await Save();
        }
    }
}
