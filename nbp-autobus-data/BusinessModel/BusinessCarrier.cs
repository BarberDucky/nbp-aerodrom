using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.BusinessModel
{
    public class BusinessCarrier
    {
        public Carrier Carrier { get; set; }
        public string UserId { get; set; }
        public IEnumerable<Ride> Rides { get; set; }
        public IEnumerable<Station> TakeOfStations { get; set; }
        public IEnumerable<Station> ArrivalStations { get; set; }
    }
}
