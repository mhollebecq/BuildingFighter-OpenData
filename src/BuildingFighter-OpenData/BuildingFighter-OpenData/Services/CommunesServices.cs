using BuildingFighter_OpenData.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingFighter_OpenData.Services
{
    public class CommunesServices
    {
        private static string _communesFileName = "communes2020.csv";
        public IEnumerable<Commune> Communes { get; }

        public CommunesServices(IWebHostEnvironment hostingEnvironment)
        {
            var communesPath = Path.Combine(hostingEnvironment.ContentRootPath, "Data", _communesFileName);
            //typecom,com,reg,dep,arr,tncc,ncc,nccenr,libelle,can,comparent
            Communes = System.IO.File.ReadAllLines(communesPath).Skip(1)
                .Select(l => l.Split(','))
                .Select(s => new Commune(typecom: s[0], com: s[1], reg: s[2], dep: s[3], arr: s[4], tncc: s[5], ncc: s[6], nccenr: s[7], libelle: s[8], can: s[9], comparent: s[10]))
                .ToList(); ;

        }
    }
}
