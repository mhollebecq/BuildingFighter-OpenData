using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BuildingFighter_OpenData.Models;
using BuildingFighter_OpenData.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingFighter_OpenData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly CommunesServices communesServices;

        public BoardController(CommunesServices communesServices)
        {
            this.communesServices = communesServices;
        }

        [HttpGet]
        [Route("commune/{insee}")]
        public async Task<IActionResult> GetCommuneByInsee(string insee)
        {
            if (string.IsNullOrEmpty(insee))
                return new BadRequestResult();
            var foundCommune = communesServices.Communes.FirstOrDefault(c => c.com == insee);
            if (foundCommune == null)
                return new NotFoundResult();
            var jsonShape = await GetCommuneShape(foundCommune);
            return Content(jsonShape, "application/json");
        }

        private async Task<string> GetCommuneShape(Commune commune)
        {
            //https://cadastre.data.gouv.fr/data/etalab-cadastre/latest/geojson/communes/31/31555/cadastre-31555-communes.json.gz
            StringBuilder sb = new StringBuilder();
            sb.Append("https://cadastre.data.gouv.fr/data/etalab-cadastre/latest/geojson/communes/");
            sb.Append(commune.dep);
            sb.Append("/");
            sb.Append(commune.com);
            sb.Append("/");
            sb.Append("cadastre-");
            sb.Append(commune.com);
            sb.Append("-feuilles.json.gz");
            using (HttpClient client = new HttpClient())
            {
                using (var responseMessage = await client.GetAsync(sb.ToString()))
                {
                    using(var contentStream = await responseMessage.Content.ReadAsStreamAsync())
                    {
                        using (var memStream = new MemoryStream())
                        {
                            using (var gZipStream = new GZipStream(contentStream, CompressionMode.Decompress))
                            {
                                CopyTo(gZipStream, memStream);
                            }
                            var result = Encoding.UTF8.GetString(memStream.ToArray());
                            return result;
                        }
                    }
                }
            }
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }
}