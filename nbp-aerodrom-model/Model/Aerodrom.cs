using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom_model
{ 
    public class Aerodrom
    {
        public String Id { get; set; }
        public String Grad { get; set; }
        public String Drzava { get; set; }
        public String Naziv { get; set; }
        public List<Let> PolazniLetovi { get; set; }
        public List<Let> DolazniLetovi { get; set; }
    }
}
