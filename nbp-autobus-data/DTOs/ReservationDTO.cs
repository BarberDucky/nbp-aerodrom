using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.DTOs
{
    public class CreateReservationDTO
    {
        public DateTime Date { get; set; }
        public List<string> RidesId { get; set; }
    }
}
