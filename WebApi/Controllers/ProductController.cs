using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: api/<ItemsController>
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_unitOfWork.Products.GetAllList());
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
                return Ok(_unitOfWork.Products.GetById(id));
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST api/<ItemsController>
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                _unitOfWork.Products.Add(product);
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
        public IActionResult Put(Guid id, [FromBody] Product productUpdated)
        {
            try
            {
                var product = _unitOfWork.Products.GetById(id);
                if (product != null && product.ProductId != null)
                {
                    product.Name = productUpdated.Name;
                    product.ProductTypeId = productUpdated.ProductTypeId;
                    product.ImageUrl = productUpdated.ImageUrl;
                    product.IsActive = productUpdated.IsActive;
                    product.ModifiedDate = DateTime.Now;
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
                var product = _unitOfWork.Products.GetById(id);
                _unitOfWork.Products.Remove(product);
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
