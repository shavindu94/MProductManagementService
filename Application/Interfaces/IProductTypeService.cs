using Application.DtoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    interface IProductTypeService
    {
        Task<List<DtoProductType>> GetProductsAsync();

        List<DtoProductType> GetProducts();

        void CreateProductType(DtoProductType dtoProductType);

        Task<DtoPagination> GetProductTypesAsync(DtoPagination paginationIn);

        Task<DtoProductType> GetByIdAsync(Guid id);

        Task UpdateProductTypeAsync(Guid id, DtoProductType dtoProductType);

        Task Delete(Guid id);
    }
}
