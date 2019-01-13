﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.Model
{
    public class Ride
    {

        public String Id { get; set; }
        public int NumberOfSeats { get; set; }
        public float RidePrice { get; set; }
        public DateTime TakeOfTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

    }
}
