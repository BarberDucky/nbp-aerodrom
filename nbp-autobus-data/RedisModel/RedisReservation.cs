using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.RedisModel
{
    public class RedisReservation
    {
        public string RideId { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfSeats { get; set; }
    }
}
