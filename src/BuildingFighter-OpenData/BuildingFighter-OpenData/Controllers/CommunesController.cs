using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingFighter_OpenData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunesController : ControllerBase
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public CommunesController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult SearchCommune(string search)
        {
            var oui = Directory.GetDirectories(hostingEnvironment.ContentRootPath);
            var non = Directory.GetFiles(hostingEnvironment.WebRootPath);
            return Content("");
        }
    }
}