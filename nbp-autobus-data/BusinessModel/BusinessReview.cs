using nbp_autobus_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nbp_autobus_data.BusinessModel
{
    public class BusinessReview
    {
        public Review Review { get; set; }
        public User User { get; set; }
        public Carrier Carrier { get; set; }
    }
}
