using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyStore_API.Data;

namespace ToyStore_API.Contracts
{
    public interface IToyRepository:IRepositoryBase<Toy>
    {
        public Task<string> GetImageFileName(int id);
    }
}
