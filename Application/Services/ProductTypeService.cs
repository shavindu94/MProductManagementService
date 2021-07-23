using Application.DtoObjects;
using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductTypeService: IProductTypeService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<DtoProductType> GetProducts()
        {
           return  _unitOfWork.ProductTypes.GetAll().Select(a=>new DtoProductType()
            {
                ProductTypeId = a.ProductTypeId,
                Name = a.Name,
                IsActive = a.IsActive,
                ImageUrl =a.ImageUrl
            }).ToList();
        }

        public async Task<List<DtoProductType>> GetProductsAsync()
        {
            return _unitOfWork.ProductTypes.GetAll().Select(a => new DtoProductType()
            {
                ProductTypeId = a.ProductTypeId,
                Name = a.Name,
                IsActive = a.IsActive,
                ImageUrl = a.ImageUrl
            }).ToList();
        }
    }
}
