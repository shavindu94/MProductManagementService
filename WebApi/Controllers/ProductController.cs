using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.DtoObjects;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("AnotherPolicy")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class ProductController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMemoryCache memoryCache, IProductService productService, ILogger<ProductController> logger)
        {
            _memoryCache = memoryCache;
            _productService = productService;
            _logger = logger;
        }


        // GET: api/<ItemsController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<DtoProduct> dtoProductList = new List<DtoProduct>();
                var cacheKey = "productList";
                if (!_memoryCache.TryGetValue(cacheKey, out dtoProductList))
                {
                    dtoProductList = await _productService.GetProductsAsync();
                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    };
                    _memoryCache.Set(cacheKey, dtoProductList, cacheExpiryOptions);
                }
                return Ok(dtoProductList);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product List Get all failed", ex);
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
                return Ok(await _productService.GetProductsAsync(pageObj));
            }
            catch (Exception ex)
            {

                _logger.LogError("Paginated List Get all failed", ex);
                return StatusCode(500);
            }

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await _productService.GetByIdAsync(id));
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation("Product Get , Couldnt find the Product Id  : " + id + "Data", ex);
                return StatusCode(401);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get by Id failed Id :" + id, ex);
                return StatusCode(500);
            }
        }

        // POST api/<ItemsController>
        [HttpPost]
        public IActionResult Post([FromBody] DtoProduct dtoProduct)
        {
            try
            {
                _productService.CreateProduct(dtoProduct);
                InvalidateProductCache();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Product save error", ex);
                return StatusCode(401);
            }
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] DtoProduct DtoProductUpdated)
        {
            try
            {
                await _productService.UpdateProductAsync(id, DtoProductUpdated);
                InvalidateProductCache();
                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation("Product Update , Couldnt find the Product Type Id  : " + id + "Data", ex);
                return StatusCode(401);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product update error", ex);
                return StatusCode(401);
            }

        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _productService.Delete(id);
                InvalidateProductCache();
                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation("Product Delete , Couldnt find the Product Id  : " + id + "Data", ex);
                return StatusCode(401);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product delete error", ex);
                return StatusCode(401);
            }
        }

        private void InvalidateProductCache()
        {
            var cacheKey = "productList";
            if (_memoryCache.TryGetValue(cacheKey, out List<DtoProduct> dtoProductList))
            {
                _memoryCache.Remove(cacheKey);
            }
        }
    }
}
