using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductTypeRepository ProductTypes { get; }
        IProductRepository Products { get; }
        IItemRepository Items { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}
