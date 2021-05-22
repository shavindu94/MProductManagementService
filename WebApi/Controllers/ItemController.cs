using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("AnotherPolicy")]
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: api/<ItemsController>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_unitOfWork.Items.GetItemList());
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
                return Ok(_unitOfWork.Items.GetById(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST api/<ItemsController>
        [HttpPost]
        public IActionResult Post([FromBody] Item item)
        {
            try
            {
                _unitOfWork.Items.Add(item);
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
        public IActionResult Put(Guid id, [FromBody] Item itemUpdate)
        {
            try
            {
                var item = _unitOfWork.Items.GetById(id);
                if (item != null && item.ItemId != null)
                {
                    item.Name = itemUpdate.Name;
                    item.ProductId = itemUpdate.ProductId;
                    item.ImageUrl = itemUpdate.ImageUrl;
                    item.IsActive = itemUpdate.IsActive;
                    item.ModifiedDate = DateTime.Now;
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
                var item = _unitOfWork.Items.GetById(id);
                _unitOfWork.Items.Remove(item);
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
