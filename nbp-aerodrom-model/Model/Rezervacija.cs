using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom_model
{
   public class Rezervacija
    {
        public String Id { get; set; }
        public List<Karta> Karte { get; set; }
        public float Cena { get; set; }
        public DateTime DatumRezervacije { get; set; }
    }
}
