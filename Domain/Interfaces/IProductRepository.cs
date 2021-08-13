using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product> 
    {
        Task<List<Product>> GetAllList();
        Task<IEnumerable<Product>> GetFilteredListAsync(string searchString = "", int pageNumber = 1, int pageSize = 10);
        Task<Product> GetByIdAsync(Guid id);
    }
}
