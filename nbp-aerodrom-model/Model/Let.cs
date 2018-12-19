using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom_model
{
    public class Let
    {
        public String Id { get; set; }
        public int BrojMesta { get; set; }
        public List<Karta> ProdateKarte { get; set; }
        public Prevoznik Prevoznik { get; set; }
        public Aerodrom PolazniAerodrom { get; set; }
        public Aerodrom DolazniAerodrom { get; set; }
        public DateTime VremePolaska { get; set; }
        public DateTime VremeDolaska { get; set; }
    }
}
