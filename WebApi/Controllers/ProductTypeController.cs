using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.DtoObjects;
using Application.Interfaces;
using Application.Services;

using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
        private readonly IMemoryCache _memoryCache;
        private readonly IProductTypeService _productTypeService;
        private readonly ILogger<ProductTypeController> _logger;

        public ProductTypeController(IMemoryCache memoryCache, IProductTypeService productTypeService , ILogger<ProductTypeController> logger)
        {
            _memoryCache = memoryCache;
            _productTypeService = productTypeService;
            _logger = logger;
        }


        // GET: api/<ItemsController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<DtoProductType> dtoProductTypeList = new List<DtoProductType>();
                var cacheKey = "productTypeList";
                if (!_memoryCache.TryGetValue(cacheKey, out dtoProductTypeList))
                {
                    dtoProductTypeList = await _productTypeService.GetProductsAsync();
                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    };
                    _memoryCache.Set(cacheKey, dtoProductTypeList, cacheExpiryOptions);
                }
                return Ok(dtoProductTypeList);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product Type List Get all failed", ex);
                return StatusCode(500);
            }
            
        }

        
        [HttpGet]
        [Route("GetPaginationList")]
        public async Task<IActionResult> GetPaginationList()
         {
            try
            {
                DtoPagination pageObj = JsonConvert.DeserializeObject<DtoPagination>(HttpContext.Request.Query["Pagination"].ToString());
                return Ok(await _productTypeService.GetProductTypesAsync(pageObj));
            }
            catch (Exception ex)
            {

                _logger.LogError("Paginated Product Type List Get all failed", ex);
                return StatusCode(500);
            }

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return Ok( await _productTypeService.GetByIdAsync(id));
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation("Couldnt find the Product Type Id  : "+id + "Data", ex);
                return StatusCode(500);
            }
            catch (Exception ex)
            {

                _logger.LogError("Get by Id failed Id :" + id, ex);
                return StatusCode(401);
            }
        }

        // POST api/<ItemsController>
        [HttpPost]
        public IActionResult Post([FromBody] DtoProductType dtoProductType)
        {
            try
            {
                _productTypeService.CreateProductType(dtoProductType);
                InvalidateProductTypeCache();
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] DtoProductType DtoProductTypeUpdated)
        {
            try
            {
                await  _productTypeService.UpdateProductTypeAsync(id, DtoProductTypeUpdated);
                InvalidateProductTypeCache();
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
               await  _productTypeService.Delete(id);
               InvalidateProductTypeCache();
               return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void InvalidateProductTypeCache()
        {
            var cacheKey = "productTypeList";
            if (_memoryCache.TryGetValue(cacheKey, out List<DtoProductType> dtoProductTypeList))
            {
                _memoryCache.Remove(cacheKey);
            }
        }

    }
}
