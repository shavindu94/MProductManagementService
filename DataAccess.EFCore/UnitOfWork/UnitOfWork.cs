using DataAccess.EFCore.Repositories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EFCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            ProductTypes = new ProductTypeRepository(_context);
            Products = new ProductRepository(_context);
            Items = new ItemRepository(_context);
        }
        public IProductTypeRepository ProductTypes { get; private set; }
        public IProductRepository Products { get; private set; }
        public IItemRepository Items { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
