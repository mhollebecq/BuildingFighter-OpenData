using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingFighter_OpenData.Models
{
    public class Commune
    {
        public string typecom { get; }
        public string com { get; }
        public string reg { get; }
        public string dep { get; }
        public string arr { get; }
        public string tncc { get; }
        public string ncc { get; }
        public string nccenr { get; }
        public string libelle { get; }
        public string can { get; }
        public string comparent { get; }

        public Commune(string typecom, string com, string reg, string dep, string arr, string tncc, string ncc, string nccenr, string libelle, string can, string comparent)
        {
            this.typecom = typecom;
            this.com = com;
            this.reg = reg;
            this.dep = dep;
            this.arr = arr;
            this.tncc = tncc;
            this.ncc = ncc;
            this.nccenr = nccenr;
            this.libelle = libelle;
            this.can = can;
            this.comparent = comparent;
        }
    }
}
