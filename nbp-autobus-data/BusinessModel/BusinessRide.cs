using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.BusinessModel
{
    public class BusinessRide
    {
        public Ride Ride { get; set; }
        public Carrier Carrier { get; set; }
        public Station TakeOfStation { get; set; }
        public Station ArrivalStation { get; set; }
    }
}
