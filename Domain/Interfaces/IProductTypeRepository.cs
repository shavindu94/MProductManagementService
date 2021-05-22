using Domain.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IProductTypeRepository :IGenericRepository<ProductType>
    {
        List<ProductType> GetAllList();
        DTOPagination GetAllList(DTOPagination dtoPaginationIn);
    }
}
