using Application.DtoObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IItemService
    {
        Task<List<DtoItem>> GetItemsAsync();

        List<DtoItem> GetItems();

        void CreateItem(DtoItem dtoItem);

        Task<DtoPagination> GetItemsAsync(DtoPagination paginationIn);

        Task<DtoItem> GetByIdAsync(Guid id);

        Task UpdateItemAsync(Guid id, DtoItem dtoItem);

        Task Delete(Guid id);
    }
}
