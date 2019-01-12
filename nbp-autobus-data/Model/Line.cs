using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.Model
{
    public class Line
    {
        public String Id { get; set; }
        public Station StartStation { get; set; }
        public Station EndStation { get; set; }
    }
}
