
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

        public async Task<IEnumerable<ProductType>> GetFilteredListAsync(string searchString = "", int pageNumber = 1, int pageSize = 10)
        {
            return await Find(a => a.Name.Contains((searchString == "" || searchString == null) ? a.Name : searchString))
                  .OrderBy(x => x.CreatedDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<ProductType> GetByIdAsync(Guid id)
        {
            return await Find(a => a.ProductTypeId.Equals(id))
                .FirstOrDefaultAsync();
        }



    }
}
