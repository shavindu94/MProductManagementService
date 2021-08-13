using Application.Common.Exceptions;
using Application.DtoObjects;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ItemService :IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContentService _contentService;

        public ItemService(IUnitOfWork unitOfWork, IContentService contentService)
        {
            _unitOfWork = unitOfWork;
            _contentService = contentService;
        }

        public List<DtoItem> GetItems()
        {
            return _unitOfWork.Items.GetAll().Select(a => new DtoItem()
            {
                ItemId = a.ItemId,
                ProductId = a.ProductId,
                Name = a.Name,
                IsActive = a.IsActive,
                ImageUrl = a.ImageUrl,
                ProductName = ((a.Product != null) ? a.Product.Name : "")
            }).ToList();
        }

        public async Task<List<DtoItem>> GetItemsAsync()
        {
            IEnumerable<Item> Items = await _unitOfWork.Items.GetAllList();
            return FillDtoItems(Items.ToList());
        }

        public void CreateItem(DtoItem dtoItem)
        {
            _unitOfWork.Items.Add(FillItem(dtoItem, new Item()));
            _unitOfWork.Complete();
        }

        public async Task<DtoPagination> GetItemsAsync(DtoPagination paginationIn)
        {
            var list = await _unitOfWork.Items.GetFilteredListAsync(paginationIn.SearchString, paginationIn.PageNumber, paginationIn.PageSize);

            DtoPagination pagination = new DtoPagination()
            {
                PageNumber = paginationIn.PageNumber,
                PageSize = paginationIn.PageSize,
                TotalNumber = _unitOfWork.Items.GetCount(),
                Data = (list != null && list.Any() ? FillDtoItems(list.ToList()) : null)
            };
            return pagination;
        }

        private List<DtoItem> FillDtoItems(List<Item> Items)
        {
            List<DtoItem> dtoItems = new List<DtoItem>();

            foreach (var Item in Items)
            {
                dtoItems.Add(FillDtoItem(Item));
            }

            return dtoItems;
        }

        private DtoItem FillDtoItem(Item item)
        {
            return new DtoItem()
            {
                ItemId = item.ItemId,
                ProductId = item.ProductId,
                Name = item.Name,
                IsActive = item.IsActive,
                ImageUrl = item.ImageUrl,
                ProductName = ((item.Product != null) ? item.Product.Name : "")
            };
        }

        private Item FillItem(DtoItem dtoItem, Item Item)
        {

            Item.Name = dtoItem.Name;
            Item.IsActive = dtoItem.IsActive;
            Item.ImageUrl = dtoItem.ImageUrl;
            Item.ProductId = dtoItem.ProductId;


            if (Item.ItemId == Guid.Empty)
            {
                Item.CreatedDate = DateTime.Now;
            }
            else
            {
                Item.ModifiedDate = DateTime.Now;
            }
            return Item;

        }

        public async Task<DtoItem> GetByIdAsync(Guid id)
        {
            Item Item = await _unitOfWork.Items.GetByIdAsync(id);

            if (Item != null && Item.ItemId != Guid.Empty)
            {
                return FillDtoItem(Item);
            }
            else
            {
                throw new NotFoundException("Item not found Id :" + id.ToString());
            }
        }


        public async Task UpdateItemAsync(Guid id, DtoItem dtoItem)
        {
            Item Item = await _unitOfWork.Items.GetByIdAsync(id);

            if (Item != null && Item.ItemId != Guid.Empty)
            {
                bool imageChanged = false;

                if (!string.IsNullOrWhiteSpace(Item.ImageUrl) && Item.ImageUrl != dtoItem.ImageUrl)
                    imageChanged = true;

                var ItemUpdated = FillItem(dtoItem, Item);
                _unitOfWork.Items.Update(ItemUpdated);
                await _unitOfWork.CompleteAsync();
                if (imageChanged)
                {
                    await _contentService.RemoveContent(Item.ImageUrl);
                }
            }
            else
            {
                throw new NotFoundException("Item not found Id:", id.ToString());
            }
        }

        public async Task Delete(Guid id)
        {
            Item Item = await _unitOfWork.Items.GetByIdAsync(id);

            if (Item != null && Item.ItemId != Guid.Empty)
            {
                _unitOfWork.Items.Remove(Item);
                _unitOfWork.Complete();
                await _contentService.RemoveContent(Item.ImageUrl);
            }
            else
            {
                throw new NotFoundException("Item not found Id :" + id.ToString());
            }

        }
    }
}
