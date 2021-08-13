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
    public class ProductService :IProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IContentService _contentService;

        public ProductService(IUnitOfWork unitOfWork, IContentService contentService)
        {
            _unitOfWork = unitOfWork;
            _contentService = contentService;
        }

        public List<DtoProduct> GetProducts()
        {
            return _unitOfWork.Products.GetAll().Select(a => new DtoProduct()
            {
                ProductId = a.ProductId,
                ProductTypeId = a.ProductTypeId,
                Name = a.Name,
                IsActive = a.IsActive,
                ImageUrl = a.ImageUrl,
                ProductTypeName = ((a.ProductType != null)? a.ProductType.Name :"")
            }).ToList();
        }

        public async Task<List<DtoProduct>> GetProductsAsync()
        {
            IEnumerable<Product> products = await _unitOfWork.Products.GetAllList();
            return FillDtoProducts(products.ToList());
        }

        public void CreateProduct(DtoProduct dtoProduct)
        {
            _unitOfWork.Products.Add(FillProduct(dtoProduct, new Product()));
            _unitOfWork.Complete();
        }

        public async Task<DtoPagination> GetProductsAsync(DtoPagination paginationIn)
        {
            var list = await _unitOfWork.Products.GetFilteredListAsync(paginationIn.SearchString, paginationIn.PageNumber, paginationIn.PageSize);

            DtoPagination pagination = new DtoPagination()
            {
                PageNumber = paginationIn.PageNumber,
                PageSize = paginationIn.PageSize,
                TotalNumber = _unitOfWork.Products.GetCount(),
                Data = (list != null && list.Any() ? FillDtoProducts(list.ToList()) : null)
            };
            return pagination;
        }

        private List<DtoProduct> FillDtoProducts(List<Product> products)
        {
            List<DtoProduct> dtoProducts = new List<DtoProduct>();

            foreach (var product in products)
            {
                dtoProducts.Add(FillDtoProduct(product));
            }

            return dtoProducts;
        }

        private DtoProduct FillDtoProduct(Product product)
        {
            return new DtoProduct()
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Name = product.Name,
                IsActive = product.IsActive,
                ImageUrl = product.ImageUrl,
                ProductTypeName = ((product.ProductType != null) ? product.ProductType.Name : "")
            };
        }

        private Product FillProduct(DtoProduct dtoProduct, Product product)
        {

            product.Name = dtoProduct.Name;
            product.IsActive = dtoProduct.IsActive;
            product.ImageUrl = dtoProduct.ImageUrl;
            product.ProductTypeId = dtoProduct.ProductTypeId;


            if (product.ProductId == Guid.Empty)
            {
                product.CreatedDate = DateTime.Now;
            }
            else
            {
                product.ModifiedDate = DateTime.Now;
            }
            return product;

        }

        public async Task<DtoProduct> GetByIdAsync(Guid id)
        {
            Product product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product != null && product.ProductId != Guid.Empty)
            {
                return FillDtoProduct(product);
            }
            else
            {
                throw new NotFoundException("Product not found Id :" + id.ToString());
            }
        }


        public async Task UpdateProductAsync(Guid id, DtoProduct dtoProduct)
        {
            Product product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product != null && product.ProductId != Guid.Empty)
            {
                bool imageChanged = false;

                if (!string.IsNullOrWhiteSpace(product.ImageUrl) && product.ImageUrl != dtoProduct.ImageUrl)
                    imageChanged = true;

                var productUpdated = FillProduct(dtoProduct, product);
                _unitOfWork.Products.Update(productUpdated);
                await _unitOfWork.CompleteAsync();
                if (imageChanged)
                {
                    await _contentService.RemoveContent(product.ImageUrl);
                }
            }
            else
            {
                throw new NotFoundException("Product not found Id:", id.ToString());
            }
        }

        public async Task Delete(Guid id)
        {
            Product product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product != null && product.ProductId != Guid.Empty)
            {
                _unitOfWork.Products.Remove(product);
                _unitOfWork.Complete();
                await _contentService.RemoveContent(product.ImageUrl);
            }
            else
            {
                throw new NotFoundException("Product not found Id :" + id.ToString());
            }

        }
    }
}
