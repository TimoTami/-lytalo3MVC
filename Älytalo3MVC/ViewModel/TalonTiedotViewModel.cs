using Älytalo3MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Älytalo3MVC.ViewModel
{
    public class TalonTiedotViewModel
    {
        public int TaloID { get; set; }
        public string TalonNimi { get; set; }

        public int SaunaID { get; set; }
        public string SaunanNimi { get; set; }
        public bool SaunanTila { get; set; }
        public int? SaunanNykyLampotila { get; set; }

        public int TaloLampoID { get; set; }
        public string HuoneenNimi { get; set; }
        public int TalonTavoiteLampotila { get; set; }
        public int TalonNykyLampotila { get; set; }

        public int ValoID { get; set; }
        public string ValonNimi { get; set; }
        public bool ValonTila { get; set; }
        public int? ValonMaara { get; set; }


        public IEnumerable<Talo> Talot { get; set; }
        public IEnumerable<Sauna> Saunat { get; set; }
        public IEnumerable<TaloLampo> TaloLampot { get; set; }
        public IEnumerable<Valo> Valot { get; set; }
    }
}