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
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationContext context) : base(context)
        {
        }


        public async Task<List<Item>> GetAllList()
        {
            return await _context.Items.Include(a => a.Product).ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetFilteredListAsync(string searchString = "", int pageNumber = 1, int pageSize = 10)
        {
            return await Find(a => a.Name.Contains((searchString == "" || searchString == null) ? a.Name : searchString))
                  .OrderBy(x => x.CreatedDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Item> GetByIdAsync(Guid id)
        {
            return await Find(a => a.ItemId.Equals(id))
                .FirstOrDefaultAsync();
        }
    }
}
