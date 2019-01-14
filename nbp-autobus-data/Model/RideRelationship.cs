using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.Model
{
    public class RideRelationship
    {
        public String Id { get; set; }
        public float RidePrice { get; set; }
        public DateTime TakeOfTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
      
        public String CarrierId { get; set; }

        public RideRelationship()
        {

        }

        public RideRelationship(Ride ride, string carrierId)
        {
            Id = ride.Id;
            RidePrice = ride.RidePrice;
            TakeOfTime = ride.TakeOfTime;
            ArrivalTime = ride.ArrivalTime;
            DayOfWeek = ride.DayOfWeek;
            CarrierId = carrierId;
           
        }
    }

}
