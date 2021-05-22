using Domain.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.EFCore.Repositories
{
    public class ProductTypeRepository : GenericRepository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository (ApplicationContext context) : base(context)
        {
        }

        public List<ProductType> GetAllList()
        {
            return _context.ProductTypes.Include(a => a.Products).ToList();
        }

        public DTOPagination GetAllList(DTOPagination dtoPaginationIn)
        {
            var list = _context.ProductTypes.Where(a=>a.Name.Contains((dtoPaginationIn.SearchString == "undefined" || dtoPaginationIn.SearchString == null) ? a.Name : dtoPaginationIn.SearchString)).
                             Include(a => a.Products).Skip((dtoPaginationIn.PageNumber - 1) * dtoPaginationIn.PageSize).Take(dtoPaginationIn.PageSize).ToList();

             DTOPagination dtoPagination =new DTOPagination() 
             { 
                PageNumber = dtoPaginationIn.PageNumber, 
                PageSize = dtoPaginationIn.PageSize,          
                TotalNumber = _context.ProductTypes.Count(), 
                Data = (list != null && list.Count != 0 ? list : null) 
             };
            return dtoPagination;
        }
    }
}
