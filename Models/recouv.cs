using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proera.Models
{
    public class recouv
    {
        public string ReferenceContrat { get; set; }
        public string numcompteur { get; set; }
        public string Ancienindex { get; set; }
        public string Nouvelindex { get; set; }
        public string consommation { get; set; }
        public string datereleve { get; set; }
        public string periode { get; set; }
        public string nombrejours { get; set; }
        public string categorie { get; set; }

    }
}