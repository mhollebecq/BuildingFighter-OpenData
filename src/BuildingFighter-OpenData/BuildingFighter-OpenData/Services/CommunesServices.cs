using BuildingFighter_OpenData.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BuildingFighter_OpenData.Services
{
    public class CommunesServices
    {
        private static string _communesFileName = "villes_france.csv";
        //private static string _communesFileName = "communes2020.csv";
        public IEnumerable<Commune> Communes { get; }

        public CommunesServices(IWebHostEnvironment hostingEnvironment)
        {
            var communesPath = Path.Combine(hostingEnvironment.ContentRootPath, "Data", _communesFileName);
            //typecom,com,reg,dep,arr,tncc,ncc,nccenr,libelle,can,comparent
            Communes = System.IO.File.ReadAllLines(communesPath).Skip(1)
                .Select(l => l.Replace("\"", string.Empty).Split(','))
                .Select(s => new Commune(typecom: "COM", com: s[10], reg: "", dep: s[1], arr: "", tncc: "0", ncc: s[3], nccenr: s[4], libelle: s[5], can: "", comparent: s[10], longi: double.Parse(s[19], CultureInfo.InvariantCulture), lat: double.Parse(s[20], CultureInfo.InvariantCulture)))
                .ToList();
            //Communes = System.IO.File.ReadAllLines(communesPath).Skip(1)
            //     .Select(l => l.Split(','))
            //     .Select(s => new Commune(typecom: s[0], com: s[1], reg: s[2], dep: s[3], arr: s[4], tncc: s[5], ncc: s[6], nccenr: s[7], libelle: s[8], can: s[9], comparent: s[10]))
            //     .ToList();

        }

        public async Task<IEnumerable<Commune>> ReverseGeoLoc(double lat, double @long)
        {
            using (HttpClient client = new HttpClient())
            {
                var baseQuery = @"https://nominatim.openstreetmap.org/reverse.php?format=geojson&lat={0}&lon={1}&zoom=10&extratags=1";
                using (var request = new HttpRequestMessage())
                {
                    request.RequestUri = new Uri(string.Format(CultureInfo.InvariantCulture, baseQuery, lat, @long));
                    request.Headers.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("(Building Fighter France - Open)"));
                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(await response.Content.ReadAsStringAsync());
                            var features = jsonObject["features"];
                            if (features.Any())
                            {
                                var feature = features[0];
                                var properties = feature["properties"];
                                var osm_id = properties["osm_id"].ToString();
                                var name = properties["name"].ToString();
                                var extratags = properties["extratags"];
                                var refInsee = extratags["ref:INSEE"].ToString();

                                return ToIEnumerable(Communes.FirstOrDefault(c => c.com == refInsee));
                            }
                        }
                    }
                }
            }

            return Enumerable.Empty<Commune>();
        }

        public IEnumerable<T> ToIEnumerable<T>(T element)
        {
            yield return element;
        }
    }
}
