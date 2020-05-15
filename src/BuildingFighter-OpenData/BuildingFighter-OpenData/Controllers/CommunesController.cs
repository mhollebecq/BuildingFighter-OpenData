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

        [HttpGet("search")]
        public IActionResult SearchCommune(string q)
        {
            return new JsonResult(
                (string.IsNullOrEmpty(q)
                    ? Enumerable.Empty<Commune>()
                    : communesServices.Communes).Where(c=>c.libelle.ToLower().Contains(q.ToLower())).ToArray());
        }

        [HttpGet("locate")]
        public async Task<IActionResult> SearchCommune(double lat, double @long)
        {
            return new JsonResult(
                (await communesServices.ReverseGeoLoc(lat,@long))
                .Take(5).ToArray());
        }

        static double GetDistanceTo((double Latitude, double Longitude) first, (double Latitude, double Longitude) other )
        {
            if (double.IsNaN(first.Latitude) || double.IsNaN(first.Longitude) || double.IsNaN(other.Latitude) ||
                double.IsNaN(other.Longitude))
            {
                throw new ArgumentException("Argument latitude or longitude is not a number");
            }

            var d1 = first.Latitude * (Math.PI / 180.0);
            var num1 = first.Longitude * (Math.PI / 180.0);
            var d2 = other.Latitude * (Math.PI / 180.0);
            var num2 = other.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

    }
}