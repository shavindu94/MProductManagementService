using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetAllList()
        { 
            return await _context.Products.Include(a => a.ProductType).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFilteredListAsync(string searchString = "", int pageNumber = 1, int pageSize = 10)
        {
            return await Find(a => a.Name.Contains((searchString == "" || searchString == null) ? a.Name : searchString))
                  .OrderBy(x => x.CreatedDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await Find(a => a.ProductId.Equals(id))
                .FirstOrDefaultAsync();
        }

    }
}
