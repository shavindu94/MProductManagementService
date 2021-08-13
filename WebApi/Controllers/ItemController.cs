using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Application.DtoObjects;
using Newtonsoft.Json;
using Application.Common.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("AnotherPolicy")]
    public class ItemController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IItemService _itemService;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IMemoryCache memoryCache, IItemService itemService, ILogger<ItemController> logger)
        {
            _memoryCache = memoryCache;
            _itemService = itemService;
            _logger = logger;
        }


        // GET: api/<ItemsController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<DtoItem> dtoItemList = new List<DtoItem>();
                var cacheKey = "ItemList";
                if (!_memoryCache.TryGetValue(cacheKey, out dtoItemList))
                {
                    dtoItemList = await _itemService.GetItemsAsync();
                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromMinutes(2)
                    };
                    _memoryCache.Set(cacheKey, dtoItemList, cacheExpiryOptions);
                }
                return Ok(dtoItemList);
            }
            catch (Exception ex)
            {
                _logger.LogError("Item List Get all failed", ex);
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
                return Ok(await _itemService.GetItemsAsync(pageObj));
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
                return Ok(await _itemService.GetByIdAsync(id));
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation("Item Get , Couldnt find the Item Id  : " + id + "Data", ex);
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
        public IActionResult Post([FromBody] DtoItem dtoItem)
        {
            try
            {
                _itemService.CreateItem(dtoItem);
                InvalidateItemCache();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Item save error", ex);
                return StatusCode(401);
            }
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] DtoItem DtoItemUpdated)
        {
            try
            {
                await _itemService.UpdateItemAsync(id, DtoItemUpdated);
                InvalidateItemCache();
                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation("Item Update , Couldnt find the Item Type Id  : " + id + "Data", ex);
                return StatusCode(401);
            }
            catch (Exception ex)
            {
                _logger.LogError("Item update error", ex);
                return StatusCode(401);
            }

        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _itemService.Delete(id);
                InvalidateItemCache();
                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation("Item Delete , Couldnt find the Item Id  : " + id + "Data", ex);
                return StatusCode(401);
            }
            catch (Exception ex)
            {
                _logger.LogError("Item delete error", ex);
                return StatusCode(401);
            }
        }

        private void InvalidateItemCache()
        {
            var cacheKey = "ItemList";
            if (_memoryCache.TryGetValue(cacheKey, out List<DtoItem> dtoItemList))
            {
                _memoryCache.Remove(cacheKey);
            }
        }
    }
}
