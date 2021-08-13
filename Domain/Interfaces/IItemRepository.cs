using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<List<Item>> GetAllList();
        Task<IEnumerable<Item>> GetFilteredListAsync(string searchString = "", int pageNumber = 1, int pageSize = 10);
        Task<Item> GetByIdAsync(Guid id);
    }
}
