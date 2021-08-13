using Application.DtoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<List<DtoProduct>> GetProductsAsync();

        List<DtoProduct> GetProducts();

        void CreateProduct(DtoProduct dtoProduct);

        Task<DtoPagination> GetProductsAsync(DtoPagination paginationIn);

        Task<DtoProduct> GetByIdAsync(Guid id);

        Task UpdateProductAsync(Guid id, DtoProduct dtoProduct);

        Task Delete(Guid id);
    }
}
