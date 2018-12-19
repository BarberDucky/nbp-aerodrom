using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom.Model
{
    enum TipPrtljaga
    {
        Handheld, 
        Regular
    }

    class Prtljag
    {
        public String Id { get; set; }
        public TipPrtljaga TipPrtljaga { get; set; }
        public float Cena { get; set; }
        public Karta Karta { get; set; }
    }
}
