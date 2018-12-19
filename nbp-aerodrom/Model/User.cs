using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom.Model
{
    class User
    {
        public String Id { get; set; }
        public String Ime { get; set; }
        public String Prezime { get; set; }
        public String BrojPasosa { get; set; }
        public List<Rezervacija> Rezervacije { get; set; }
        public List<Rezervacija> PlaceneRezervacije { get; set; }
    }
}
