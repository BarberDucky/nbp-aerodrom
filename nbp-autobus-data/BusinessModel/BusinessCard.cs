using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.BusinessModel
{
    public class BusinessCard
    {
        public Card Card { get; set; }
        public List<BusinessRide> Rides { get; set; }
        public Station TakeOfStation { get; set; }
        public Station ArrivalStation { get; set; }
        //public string CarrierId { get; set; }
        //public string CarrierName { get; set; }

        public BusinessCard()
        {
            Card = new Card()
            {
                Price = 0
            };
            Rides = new List<BusinessRide>();
        }
    }
}
