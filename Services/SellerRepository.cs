using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyStore_API.Contracts;
using ToyStore_API.Data;

namespace ToyStore_API.Services
{
    public class SellerRepository : ISellerRepository
    {
        private readonly ApplicationDbContext _db;

        public SellerRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Seller entity)
        {
           await _db.Sellers.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Seller entity)
        {
            _db.Sellers.Remove(entity);
            return await Save();
        }

        public async Task<IList<Seller>>FindAll()
        {
            var sellers = await _db.Sellers
               .Include(q => q.Toys)
               .ToListAsync();
            return sellers;
        }

        public async Task<Seller> FindById(int id)
        {
            var seller = await _db.Sellers
                .Include(q => q.Toys)
                .FirstOrDefaultAsync(q => q.Id == id);
            return seller;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Sellers.AnyAsync(q => q.Id ==id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;

        }

        public async Task<bool> Update(Seller entity)
        {
            _db.Sellers.Update(entity);
            return await Save();
        }

       
    }
}
