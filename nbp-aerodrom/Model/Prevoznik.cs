using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_aerodrom.Model
{
    class Prevoznik
    {
        public String Id { get; set; }
        public String Naziv { get; set; }
        public String WebSajt { get; set; }
        public List<Let> Letovi { get; set; }
    }
}
