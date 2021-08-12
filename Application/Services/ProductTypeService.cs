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
    public class ProductTypeService : IProductTypeService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IContentService _contentService;

        public ProductTypeService(IUnitOfWork unitOfWork , IContentService contentService)
        {
            _unitOfWork = unitOfWork;
            _contentService = contentService;
        }

        public List<DtoProductType> GetProducts()
        {
            return _unitOfWork.ProductTypes.GetAll().Select(a => new DtoProductType()
            {
                ProductTypeId = a.ProductTypeId,
                Name = a.Name,
                IsActive = a.IsActive,
                ImageUrl = a.ImageUrl
            }).ToList();
        }

        public async Task<List<DtoProductType>> GetProductsAsync()
        {
            IEnumerable<ProductType> productTypes = await _unitOfWork.ProductTypes.GetAllAsync();
            return FillDtoProductTypes(productTypes.ToList());
        }

        public void CreateProductType(DtoProductType dtoProductType)
        {
            _unitOfWork.ProductTypes.Add(FillProductType(dtoProductType, new ProductType()));
            _unitOfWork.Complete();
        }

        public async Task<DtoPagination> GetProductTypesAsync(DtoPagination paginationIn)
        {
            var list = await _unitOfWork.ProductTypes.GetFilteredListAsync(paginationIn.SearchString, paginationIn.PageNumber, paginationIn.PageSize);

            DtoPagination pagination = new DtoPagination()
            {
                PageNumber = paginationIn.PageNumber,
                PageSize = paginationIn.PageSize,
                TotalNumber = _unitOfWork.ProductTypes.GetCount(),
                Data = (list != null && list.Any() ? FillDtoProductTypes(list.ToList()) : null)
            };
            return pagination;
        }

        private List<DtoProductType> FillDtoProductTypes(List<ProductType> productTypes)
        {
            List<DtoProductType> dtoProductTypes = new List<DtoProductType>();

            foreach (var productType in productTypes)
            {
                dtoProductTypes.Add(FillDtoProductType(productType));
            }

            return dtoProductTypes;
        }

        private DtoProductType FillDtoProductType(ProductType productType)
        {

            return new DtoProductType()
            {
                ProductTypeId = productType.ProductTypeId,
                Name = productType.Name,
                IsActive = productType.IsActive,
                ImageUrl = productType.ImageUrl
            };

        }

        private ProductType FillProductType(DtoProductType dtoProductType , ProductType productType)
        {

            productType.Name = dtoProductType.Name;
            productType.IsActive = dtoProductType.IsActive;
            productType.ImageUrl = dtoProductType.ImageUrl;
            

            if (productType.ProductTypeId == Guid.Empty)
            {
                productType.CreatedDate = DateTime.Now;
            }
            else
            {
                productType.ModifiedDate = DateTime.Now;
            }
            return productType;

        }

        public async Task<DtoProductType> GetByIdAsync(Guid id)
        {
            ProductType productType = await _unitOfWork.ProductTypes.GetByIdAsync(id);

            if (productType != null && productType.ProductTypeId != Guid.Empty)
            {
                return FillDtoProductType(productType);
            }
            else
            {
                throw new NotFoundException("Product Type not found Id :" + id.ToString());
            }
        }


        public async Task UpdateProductTypeAsync(Guid id , DtoProductType dtoProductType)
        {
            ProductType productType = await _unitOfWork.ProductTypes.GetByIdAsync(id);

            if (productType != null && productType.ProductTypeId != Guid.Empty)
            {
                bool imageChanged = false;

                if (!string.IsNullOrWhiteSpace(productType.ImageUrl) &&  productType.ImageUrl != dtoProductType.ImageUrl)
                    imageChanged = true;

                      var productTypeUpdated = FillProductType(dtoProductType,productType);
                     _unitOfWork.ProductTypes.Update(productTypeUpdated);
                      await _unitOfWork.CompleteAsync();
                if (imageChanged)
                {
                    await _contentService.RemoveContent(productType.ImageUrl);
                }
            }
            else
            {
                throw new NotFoundException("Product Type Not found Id:", id.ToString());
            }
        }

        public async Task Delete(Guid id)
        {
            ProductType productType = await _unitOfWork.ProductTypes.GetByIdAsync(id);

            if (productType != null && productType.ProductTypeId != Guid.Empty)
            {
                _unitOfWork.ProductTypes.Remove(productType);
                _unitOfWork.Complete();
                await  _contentService.RemoveContent(productType.ImageUrl);
            }
            else
            {
                throw new NotFoundException("Product Type not found Id :" + id.ToString());
            }

        }
    }
}
