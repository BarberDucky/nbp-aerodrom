using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom_model
{
   public enum TipKarte
    {
        Economy,
        PremiumEconomy,
        Business,
        FirstClass
    }

    public class Karta
    {
        public Let Let { get; set; }
        public int BrojSedista { get; set; }
        public float Cena { get; set; }
        public TipKarte TipKarte { get; set; }
        public List<Prtljag> Prtljag { get; set; }
    }
}
