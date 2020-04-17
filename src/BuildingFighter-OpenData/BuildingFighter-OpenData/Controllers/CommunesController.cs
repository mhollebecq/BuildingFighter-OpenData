using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BuildingFighter_OpenData.Models;
using BuildingFighter_OpenData.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingFighter_OpenData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunesController : ControllerBase
    {
        private readonly CommunesServices communesServices;

        public CommunesController(CommunesServices communesServices)
        {
            this.communesServices = communesServices;
        }

        [HttpGet]
        public IActionResult SearchCommune(string search)
        {
            var lowerSearch = search.ToLower();
            return new JsonResult(
                (string.IsNullOrEmpty(search)
                    ? Enumerable.Empty<Commune>()
                    : communesServices.Communes).Where(c=>c.libelle.ToLower().Contains(lowerSearch)).ToArray());
        }     
    }
}