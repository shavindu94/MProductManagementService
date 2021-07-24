using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductTypeRepository :IGenericRepository<ProductType>
    {
        List<ProductType> GetAllList();
        Task<IEnumerable<ProductType>> GetFilteredListAsync(string searchString = "", int pageNumber = 1, int pageSize = 10);
        Task<ProductType> GetByIdAsync(Guid id);
    }
}
