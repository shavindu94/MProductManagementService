
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {

        private IWebHostEnvironment _hostingEnvironment;
        public ContentController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }


        //public async Task<string> CreateImage([FromForm] DTOImage img)
        //{
        //    try
        //    {
        //        IFormFile image = img.Image;
        //        string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

        //            if (image.Length > 0)
        //            {
        //                string filePath = Path.Combine(uploads, image.FileName);
        //                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await image.CopyToAsync(fileStream);
        //                }
        //            }


        //        return "";

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
