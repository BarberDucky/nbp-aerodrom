using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nbp_autobus_data.RedisModel;

namespace nbp_autobus_data.DTOs
{
    public class SearchDTO
    {
        public String TakeOfStationId { get; set; }
        public String ArrivalStationId { get; set; }
        public DateTime TakeOfDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int NumberOfCards { get; set; }

        public static RedisSearch FromDTO(SearchDTO search)
        {
            return new RedisSearch(search);
        }
    }
}
