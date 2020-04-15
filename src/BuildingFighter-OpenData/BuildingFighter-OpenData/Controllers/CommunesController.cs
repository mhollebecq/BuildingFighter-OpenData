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
        private readonly List<(string typecom, string com, string reg, string dep, string arr, string tncc, string ncc, string nccenr, string libelle, string can, string comparent)> communes;
        private static string _communesFileName = "communes2020.csv";

        public CommunesController(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            var communesPath = Path.Combine(hostingEnvironment.ContentRootPath, "Data", _communesFileName);
            //typecom,com,reg,dep,arr,tncc,ncc,nccenr,libelle,can,comparent
            communes = System.IO.File.ReadAllLines(communesPath).Skip(1)
                .Select(l => l.Split(','))
                .Select(s => (typecom: s[0], com: s[1], reg: s[2], dep: s[3], arr: s[4], tncc: s[5], ncc: s[6], nccenr: s[7], libelle: s[8], can: s[9], comparent: s[10]))
                .ToList(); ;

        }

        [HttpGet]
        public IActionResult SearchCommune(string search)
        {
            var lowerSearch = search.ToLower();
            return new JsonResult(
                string.IsNullOrEmpty(search) 
                    ? new string[0]
                    : communes.Where(c=>c.libelle.ToLower().Contains(lowerSearch)).Select(c=>c.libelle).ToArray());
        }
    }
}