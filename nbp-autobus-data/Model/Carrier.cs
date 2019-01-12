using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.Model
{
    public class Carrier
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Website { get; set; }
        public String PhoneNumber { get; set; }
        public User User { get; set; }
        public List<Ride> Rides { get; set; }
    }
}
