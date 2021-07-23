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
            GetAllGrouped();
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

        public void GetAllGrouped()
        {
            var result = _context.Products.GroupBy(a => new { a.ProductTypeId, a.ProductType.Name }).
                Select(a => new {ProductTypeId=a.Key.ProductTypeId, ProductTypeName= a.Key.Name, ProductCount= a.Count()}).OrderBy(a=>a.ProductCount).ToList();

            var result2 = (from p in _context.Products
                          join pt in _context.ProductTypes
                          on p.ProductTypeId equals pt.ProductTypeId into x
                          from x1 in x
                          group x1 by new { x1.ProductTypeId, x1.Name } into g
                          select new
                          {
                              ProductTypeId = g.Key.ProductTypeId,
                              ProductTypeName = g.Key.Name,
                              ProductCount = g.Count()
                          }).OrderByDescending(a=>a.ProductCount).ToList();


        }



    }
}
