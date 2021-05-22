using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Http;
using Domain.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("AnotherPolicy")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class ProductTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        public ProductTypeController(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
        }


        // GET: api/<ItemsController>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {

                List<ProductType> productTypeList = _unitOfWork.ProductTypes.GetAllList().ToList();
                return Ok(productTypeList);

                //Cache 
                //var cacheKey = "productTypeList";
                //if (!_memoryCache.TryGetValue(cacheKey, out List<ProductType> productTypeList))
                //{
                //    productTypeList = _unitOfWork.ProductTypes.GetAll().ToList();
                //    var cacheExpiryOptions = new MemoryCacheEntryOptions
                //    {
                //        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                //        Priority = CacheItemPriority.High,
                //        SlidingExpiration = TimeSpan.FromMinutes(2)
                //    };
                //    _memoryCache.Set(cacheKey, productTypeList, cacheExpiryOptions);
                //}
                //return Ok(productTypeList);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        
        [HttpGet]
        [Route("GetPaginationList")]
        public IActionResult GetPaginationList()
         {
            try
            {

                DTOPagination pageObj = JsonConvert.DeserializeObject<DTOPagination>(HttpContext.Request.Query["Pagination"].ToString());


                return Ok(_unitOfWork.ProductTypes.GetAllList(pageObj));

            }
            catch (Exception)
            {

                throw;
            }

        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                return Ok(_unitOfWork.ProductTypes.GetById(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST api/<ItemsController>
        [HttpPost]
        public IActionResult Post([FromBody] ProductType productType)
        {
            try
            {
                _unitOfWork.ProductTypes.Add(productType);
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] ProductType productTypeUpdated)
        {
            try
            {
               var productType = _unitOfWork.ProductTypes.GetById(id);
                if(productType != null && productType.ProductTypeId != null)
                {
                    productType.Name = productTypeUpdated.Name;
                    productType.ImageUrl = productTypeUpdated.ImageUrl;
                    productType.IsActive = productTypeUpdated.IsActive;
                    productType.ModifiedDate = DateTime.Now;
                }
               // _unitOfWork.ProductTypes.Add(productType);
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var ProductType = _unitOfWork.ProductTypes.GetById(id);
                _unitOfWork.ProductTypes.Remove(ProductType);
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
