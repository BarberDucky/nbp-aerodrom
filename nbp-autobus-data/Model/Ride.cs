using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.Model
{
    public enum RideType
    {
        Bus,
        MiniBus,
        Car
    }

    public class Ride
    {
        public String Id { get; set; }
        public RideType RideType { get; set; }
        public int NumberOfSeats { get; set; }
        public float RidePrice { get; set; }
        public DateTime TakeOfTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
       
        public Ride()
        {

        }

        public Ride(RideRelationship ride)
        {
            Id = ride.Id;
            RidePrice = ride.RidePrice;
            TakeOfTime = ride.TakeOfTime;
            ArrivalTime = ride.ArrivalTime;
            DayOfWeek = ride.DayOfWeek;
            RideType = ride.RideType;
         
        }
    }
}
