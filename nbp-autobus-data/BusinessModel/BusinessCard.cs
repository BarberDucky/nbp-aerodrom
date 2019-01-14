using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.BusinessModel
{
    public class BusinessCard
    {
        public float Price { get; set; }
        public List<BusinessRide> Rides { get; set; }
        public string CarrierId { get; set; }

        public BusinessCard()
        {
            Price = 0;
            Rides = new List<BusinessRide>();
        }
    }
}
