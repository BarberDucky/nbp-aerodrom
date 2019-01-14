using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.DTOs
{
    public class SearchDTO
    {
        public String TakeOfStationId { get; set; }
        public String ArrivalStationId { get; set; }
        public DateTime TakeOfDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int NumberOfCards { get; set; }
    }
}
