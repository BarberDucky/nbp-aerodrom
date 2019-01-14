using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.Model
{
    public class Card
    {
        public String Id { get; set; }
        public int SeatNumber { get; set; }
        public float Price { get; set; }
        public DateTime TakeOfDate { get; set; }
    }
}
