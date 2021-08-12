
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using Application.Interfaces;
using Application.DtoObjects;
using Microsoft.AspNetCore.Cors;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("AnotherPolicy")]
    public class ContentController : ControllerBase
    {

        private IWebHostEnvironment _hostingEnvironment;

        private readonly IContentService _contentService;
        public ContentController(IWebHostEnvironment environment, IContentService contentService)
        {
            _hostingEnvironment = environment;
            _contentService = contentService;
        }

        [HttpPost]
        public async Task<string> Post([FromForm] FileModel fileModel)
        {
            try
            {
                return await _contentService.SaveContent(fileModel);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
